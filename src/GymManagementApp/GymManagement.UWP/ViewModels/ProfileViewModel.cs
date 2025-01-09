using System;
using System.Threading.Tasks;
using GymManagement.Domain;
using GymManagement.Domain.Models;
using GymManagement.Domain.Repository;
using Windows.Storage;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using System.Windows.Input;
using System.IO;
using System.Diagnostics;
using GymManagement.UWP.Helpers;
using GymManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;


namespace GymManagement.UWP.ViewModels
{
    public class ProfileViewModel : BindableBase
    {
        private string _fullName;
        private string _username;
        private string _email;
        private string _phoneNumber;
        private float _weight;
        private float _height;
        private Membership _membership;
        private int _remainingWorkouts;
        private string _profilePictureUri;
        private byte[] _image;

        public ICommand UploadImageCommand { get; }
        public ICommand ToggleEditCommand { get; }
        public ICommand SaveChangesCommand { get; }

        private readonly IUnitOfWork _unitOfWork;
        private readonly UserViewModel _userViewModel;

        public bool IsMember => UserRole == Role.Member;
        public bool IsTrainer => UserRole == Role.Trainer;
        public bool IsAdmin => UserRole == Role.Admin;

        private Role _userRole;

        private bool _isEditing;
        public bool IsEditing
        {
            get => _isEditing;
            set
            {
                if (Set(ref _isEditing, value))
                {
                    OnPropertyChanged(nameof(ButtonContent));
                }
            }
        }
        public string ButtonContent => IsEditing ? "Save" : "Edit";

        public ProfileViewModel(IUnitOfWork unitOfWork, UserViewModel userViewModel)
        {
            _unitOfWork = unitOfWork;
            _userViewModel = userViewModel;

            ToggleEditCommand = new RelayCommand(ToggleEditMode);
            UploadImageCommand = new RelayCommand(async () => await UploadImageAsync());
            SaveChangesCommand = new RelayCommand(async () => await SaveChangesAsync());

            LoadProfileFromDatabase();
        }

        private async void ToggleEditMode()
        {
            if (IsEditing)
            {
                // Save changes to the database when exiting edit mode
                await SaveChangesAsync();
            }

            // Toggle the editing mode
            IsEditing = !IsEditing;

            // Show/hide Upload button dynamically based on editing mode
            OnPropertyChanged(nameof(IsEditing));
        }



        public Role UserRole
        {
            get => _userRole;
            set => Set(ref _userRole, value);
        }


        public string FullName
        {
            get => _fullName;
            set => Set(ref _fullName, value);
        }

        public string Username
        {
            get => _username;
            set => Set(ref _username, value);
        }

        public string Email
        {
            get => _email;
            set => Set(ref _email, value);
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set => Set(ref _phoneNumber, value);
        }

        public float Weight
        {
            get => _weight;
            set => Set(ref _weight, value);
        }

        public float Height
        {
            get => _height;
            set => Set(ref _height, value);
        }

        public Membership Membership
        {
            get => _membership;
            set => Set(ref _membership, value);
        }

        public int RemainingWorkouts
        {
            get => _remainingWorkouts;
            set => Set(ref _remainingWorkouts, value);
        }

        public byte[] Image
        {
            get => _image;
            set
            {
                if (Set(ref _image, value))
                {
                    UpdateImageUri();
                }
            }
        }

        public string ProfilePictureUri
        {
            get => _profilePictureUri;
            private set => Set(ref _profilePictureUri, value);
        }

        private async void UpdateImageUri()
        {
            if (Image != null)
            {
                ProfilePictureUri = await SaveImageToFile(Image);
            }
            else
            {
                ProfilePictureUri = null; // Handle no image case
            }
        }

        public bool IsProfileComplete =>
            !string.IsNullOrWhiteSpace(FullName) &&
            !string.IsNullOrWhiteSpace(Username) &&
            !string.IsNullOrWhiteSpace(Email) &&
            PhoneNumber.Length >= 10 &&
            Weight > 0 &&
            Height > 0;

        private async Task UploadImageAsync()
        {
            try
            {
                // Open file picker
                var picker = new FileOpenPicker
                {
                    ViewMode = PickerViewMode.Thumbnail,
                    SuggestedStartLocation = PickerLocationId.PicturesLibrary
                };
                picker.FileTypeFilter.Add(".png");
                picker.FileTypeFilter.Add(".jpg");
                picker.FileTypeFilter.Add(".jpeg");

                // Let the user pick a file
                StorageFile file = await picker.PickSingleFileAsync();
                if (file != null)
                {
                    // Read the file as a byte array
                    using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read))
                    {
                        var buffer = new byte[stream.Size];
                        using (var reader = new DataReader(stream))
                        {
                            await reader.LoadAsync((uint)stream.Size);
                            reader.ReadBytes(buffer);
                        }

                        // Save the image to the local folder and update the URI
                        ProfilePictureUri = await SaveImageToFile(buffer);

                        // Update the image in the database
                        Image = buffer;
                        await SaveImageToDatabase(buffer);
                    }

                    // Notify the UI to refresh
                    OnPropertyChanged(nameof(ProfilePictureUri));
                }
            }
            catch (Exception ex)
            {
                // Handle any errors
                Debug.WriteLine($"Error uploading image: {ex.Message}");
            }
        }

        private async Task SaveImageToDatabase(byte[] imageData)
        {
            if (_userViewModel.LoggedUser is Member member)
            {
                member.Image = imageData; // Assign image data to the user
                await _unitOfWork.Members.SaveChangesAsync(); // Commit the changes to the database
            }
            else if (_userViewModel.LoggedUser is Trainer trainer)
            {
                trainer.Image = imageData;
                await _unitOfWork.Trainers.SaveChangesAsync();
            }
        }


        // Fetch user profile from the database
        private async void LoadProfileFromDatabase()
        {
            try
            {
                string userEmail = string.Empty;

                if (_userViewModel.LoggedUser is Member member)
                {
                    userEmail = member.Email;
                    var fetchedMember = await _unitOfWork.Members.GetMemberByEmailAsync(userEmail);
                    if (fetchedMember != null)
                    {
                        PopulateProfile(fetchedMember);
                    }
                }
                else if (_userViewModel.LoggedUser is Trainer trainer)
                {
                    userEmail = trainer.Email;
                    var fetchedTrainer = await _unitOfWork.Trainers.GetTrainerByEmailAsync(userEmail);
                    if (fetchedTrainer != null)
                    {
                        PopulateProfile(fetchedTrainer);
                    }
                }
                else if (_userViewModel.LoggedUser is Admin admin)
                {
                    userEmail = admin.Email;
                    // var fetchedAdmin = await _unitOfWork.Admins.GetAdminByEmailAsync(userEmail);
                    // PopulateProfile(fetchedAdmin);
                }
            }
            catch (Exception ex)
            {
                FullName = "Error Loading Profile";
                Email = "Error";
                // Log the exception if necessary
            }
        }

        private async Task<string> SaveImageToFile(byte[] imageData)
        {
            // Create a unique file name
            string fileName = $"ProfileImage_{Guid.NewGuid()}.png";
            var storageFolder = ApplicationData.Current.LocalFolder;
            var file = await storageFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);

            // Write byte array to the file
            using (var stream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                using (var outputStream = stream.GetOutputStreamAt(0))
                {
                    await outputStream.WriteAsync(imageData.AsBuffer());
                    await outputStream.FlushAsync();
                }
            }

            // Return the URI to the saved image file
            return $"ms-appdata:///local/{fileName}";
        }


        // Populate ProfileViewModel with data from the database
        private void PopulateProfile(dynamic user)
        {
            FullName = user.FullName;
            Username = user.Username;
            Email = user.Email;
            PhoneNumber = user.PhoneNumber;
            Weight = user.Weight;
            Height = user.Height;
            Membership = user.Membership;
            RemainingWorkouts = user.RemainingWorkouts;
            Image = user.Image;

            if (user is Member) UserRole = Role.Member;
            else if (user is Trainer) UserRole = Role.Trainer;
            else if (user is Admin) UserRole = Role.Admin;

            OnPropertyChanged(nameof(IsMember));
            OnPropertyChanged(nameof(IsTrainer));
            OnPropertyChanged(nameof(IsAdmin));

            OnPropertyChanged(nameof(FullName));
            OnPropertyChanged(nameof(Email));
            OnPropertyChanged(nameof(IsProfileComplete));
            OnPropertyChanged(nameof(ProfilePictureUri)); 
        }

        private async Task SaveChangesAsync()
        {
            try
            {
                if (_userViewModel.LoggedUser is Member member)
                {
                    // Fetch the existing member by email
                    var existingMember = await _unitOfWork.Members.GetMemberByEmailAsync(member.Email);
                    if (existingMember == null)
                    {
                        Debug.WriteLine($"Member with email {member.Email} not found in the database.");
                        return;
                    }

                    // Update properties
                    existingMember.FullName = FullName;
                    existingMember.Username = Username;
                    existingMember.Email = Email;
                    existingMember.PhoneNumber = PhoneNumber;
                    existingMember.Weight = Weight;
                    existingMember.Height = Height;

                    // Update image if provided
                    if (Image != null)
                    {
                        existingMember.Image = Image;
                    }

                    // Attach the entity and mark it as modified
                    _unitOfWork.AttachAsModified(existingMember);
                }
                else if (_userViewModel.LoggedUser is Trainer trainer)
                {
                    // Fetch the existing trainer by email
                    var existingTrainer = await _unitOfWork.Trainers.GetTrainerByEmailAsync(trainer.Email);
                    if (existingTrainer == null)
                    {
                        Debug.WriteLine($"Trainer with email {trainer.Email} not found in the database.");
                        return;
                    }

                    // Update properties
                    existingTrainer.FullName = FullName;
                    existingTrainer.Username = Username;
                    existingTrainer.Email = Email;
                    existingTrainer.PhoneNumber = PhoneNumber;

                    // Update image if provided
                    if (Image != null)
                    {
                        existingTrainer.Image = Image;
                    }

                    // Attach the entity and mark it as modified
                    _unitOfWork.AttachAsModified(existingTrainer);
                }

                // Commit changes to the database
                Debug.WriteLine("Saving changes to the database...");
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error saving changes: {ex.Message}");
            }
        }
    }
}
