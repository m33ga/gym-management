﻿using System;
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
using Windows.UI.Xaml.Media.Imaging;
using Windows.Graphics.Imaging;
using Windows.ApplicationModel.Core;


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


        private readonly IUnitOfWork _unitOfWork;
        private readonly UserViewModel _userViewModel;
        private readonly DbContext _dbContext;
        private double _averageRating;
        private BitmapImage _profilePictureBitmap;

        public ICommand UploadImageCommand { get; }
        public ICommand ToggleEditCommand { get; }
        public ICommand SaveChangesCommand { get; }

        private Role _userRole;
        public bool IsMember => UserRole == Role.Member;
        public bool IsTrainer => UserRole == Role.Trainer;
        public bool IsAdmin => UserRole == Role.Admin;
        public string FormattedAverageRating => AverageRating.ToString("F1");


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
            Debug.WriteLine($"Toggling edit mode. Current IsEditing: {IsEditing}");

            if (IsEditing)
            {
                // Save changes when exiting edit mode
                await SaveChangesAsync();
            }

            IsEditing = !IsEditing;

            Debug.WriteLine($"New IsEditing: {IsEditing}");

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
            set => Set(ref _image, value);
        }

        public BitmapImage ProfilePictureBitmap
        {
            get => _profilePictureBitmap;
            set
            {
                _profilePictureBitmap = value;
                OnPropertyChanged(nameof(ProfilePictureBitmap));
            }
        }
        public double AverageRating
        {
            get => _averageRating;
            set
            {
                if (Set(ref _averageRating, value))
                {
                    OnPropertyChanged(nameof(FormattedAverageRating)); // Notify UI of formatted value change
                }
            }
        }


        public bool IsProfileComplete =>
            !string.IsNullOrWhiteSpace(FullName) &&
            !string.IsNullOrWhiteSpace(Username) &&
            !string.IsNullOrWhiteSpace(Email) &&
            PhoneNumber.Length >= 10 &&
            Weight > 0 &&
            Height > 0;

        //private async Task UploadImageAsync()
        //{
        //    var picker = new FileOpenPicker
        //    {
        //        SuggestedStartLocation = PickerLocationId.PicturesLibrary
        //    };
        //    picker.FileTypeFilter.Add(".jpg");
        //    picker.FileTypeFilter.Add(".png");

        //    var file = await picker.PickSingleFileAsync();
        //    if (file != null)
        //    {
        //        try
        //        {
        //            using (var stream = await file.OpenAsync(FileAccessMode.Read))
        //            {
        //                var decoder = await BitmapDecoder.CreateAsync(stream);

        //                // Resize the image
        //                const uint targetWidth = 100;
        //                const uint targetHeight = 100;

        //                var transform = new BitmapTransform
        //                {
        //                    ScaledWidth = targetWidth,
        //                    ScaledHeight = targetHeight
        //                };

        //                var pixelData = await decoder.GetPixelDataAsync(
        //                    BitmapPixelFormat.Bgra8,
        //                    BitmapAlphaMode.Premultiplied,
        //                    transform,
        //                    ExifOrientationMode.RespectExifOrientation,
        //                    ColorManagementMode.DoNotColorManage
        //                );

        //                byte[] resizedImageBytes;
        //                using (var resizedStream = new InMemoryRandomAccessStream())
        //                {
        //                    var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, resizedStream);
        //                    encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied,
        //                                         targetWidth, targetHeight, decoder.DpiX, decoder.DpiY, pixelData.DetachPixelData());
        //                    await encoder.FlushAsync();

        //                    resizedImageBytes = new byte[resizedStream.Size];
        //                    using (var dataReader = new DataReader(resizedStream.GetInputStreamAt(0)))
        //                    {
        //                        await dataReader.LoadAsync((uint)resizedStream.Size);
        //                        dataReader.ReadBytes(resizedImageBytes);
        //                    }
        //                }

        //                // Save to the local folder
        //                string localImagePath;
        //                if (_userViewModel.LoggedUser is Member member)
        //                {
        //                    localImagePath = Path.Combine(ApplicationData.Current.LocalFolder.Path, $"{member.Email}_ProfilePicture.jpg");
        //                    await File.WriteAllBytesAsync(localImagePath, resizedImageBytes);

        //                    // Save to the database
        //                    member.Image = resizedImageBytes;
        //                    _unitOfWork.AttachAsModified(member);
        //                    await _unitOfWork.SaveChangesAsync();
        //                    Debug.WriteLine($"Saving image to path: {localImagePath}");
        //                }
        //                else if (_userViewModel.LoggedUser is Trainer trainer)
        //                {
        //                    localImagePath = Path.Combine(ApplicationData.Current.LocalFolder.Path, $"{trainer.Email}_ProfilePicture.jpg");
        //                    await File.WriteAllBytesAsync(localImagePath, resizedImageBytes);

        //                    // Save to the database
        //                    trainer.Image = resizedImageBytes;
        //                    _unitOfWork.AttachAsModified(trainer);
        //                    await _unitOfWork.SaveChangesAsync();
        //                }
        //                else
        //                {
        //                    throw new InvalidOperationException("Logged user is neither a Member nor a Trainer.");
        //                }

        //                // Update UI
        //                await CoreApplication.MainView.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
        //                {
        //                    var bitmapImage = new BitmapImage();
        //                    using (var memoryStream = new MemoryStream(resizedImageBytes))
        //                    {
        //                        await bitmapImage.SetSourceAsync(memoryStream.AsRandomAccessStream());
        //                        ProfilePictureBitmap = bitmapImage;
        //                    }
        //                });

        //                Debug.WriteLine("Image uploaded, resized, saved locally, and saved to the database.");
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Debug.WriteLine($"Error uploading image: {ex.Message}");
        //        }
        //    }
        //    else
        //    {
        //        Debug.WriteLine("No file selected.");
        //    }
        //}

        private async Task UploadImageAsync()
        {
            var picker = new FileOpenPicker
            {
                SuggestedStartLocation = PickerLocationId.PicturesLibrary
            };
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".png");

            var file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                try
                {
                    Debug.WriteLine("File selected. Starting image processing...");

                    using (var stream = await file.OpenAsync(FileAccessMode.Read))
                    {
                        var decoder = await BitmapDecoder.CreateAsync(stream);
                        Debug.WriteLine($"Decoder created. Original Width: {decoder.PixelWidth}, Height: {decoder.PixelHeight}");

                        // Resize the image
                        const uint targetWidth = 100;
                        const uint targetHeight = 100;

                        var transform = new BitmapTransform
                        {
                            ScaledWidth = targetWidth,
                            ScaledHeight = targetHeight
                        };

                        var pixelData = await decoder.GetPixelDataAsync(
                            BitmapPixelFormat.Bgra8,
                            BitmapAlphaMode.Premultiplied,
                            transform,
                            ExifOrientationMode.RespectExifOrientation,
                            ColorManagementMode.DoNotColorManage
                        );

                        Debug.WriteLine("Image resized successfully.");

                        byte[] resizedImageBytes;
                        using (var resizedStream = new InMemoryRandomAccessStream())
                        {
                            var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, resizedStream);
                            encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied,
                                                 targetWidth, targetHeight, decoder.DpiX, decoder.DpiY, pixelData.DetachPixelData());
                            await encoder.FlushAsync();

                            resizedImageBytes = new byte[resizedStream.Size];
                            using (var dataReader = new DataReader(resizedStream.GetInputStreamAt(0)))
                            {
                                await dataReader.LoadAsync((uint)resizedStream.Size);
                                dataReader.ReadBytes(resizedImageBytes);
                            }
                        }

                        Debug.WriteLine($"Image resized byte array prepared. Length: {resizedImageBytes.Length}");

                        // Save to the local folder
                        string localImagePath;
                        if (_userViewModel.LoggedUser is Member member)
                        {
                            localImagePath = Path.Combine(ApplicationData.Current.LocalFolder.Path, $"{member.Email}_ProfilePicture.jpg");
                            Debug.WriteLine($"Saving image for Member to: {localImagePath}");

                            try
                            {
                                await File.WriteAllBytesAsync(localImagePath, resizedImageBytes);
                                Debug.WriteLine("Image saved to local folder for Member successfully.");

                                // Save to the database
                                member.Image = resizedImageBytes;
                                _unitOfWork.AttachAsModified(member);
                                await _unitOfWork.SaveChangesAsync();
                                Debug.WriteLine("Member entity updated and saved to database.");
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine($"Error saving image for Member: {ex.Message}");
                                throw;
                            }
                        }
                        else if (_userViewModel.LoggedUser is Trainer trainer)
                        {
                            localImagePath = Path.Combine(ApplicationData.Current.LocalFolder.Path, $"{trainer.Email}_ProfilePicture.jpg");
                            Debug.WriteLine($"Saving image for Trainer to: {localImagePath}");

                            try
                            {
                                await File.WriteAllBytesAsync(localImagePath, resizedImageBytes);
                                Debug.WriteLine("Image saved to local folder for Trainer successfully.");

                                // Save to the database
                                trainer.Image = resizedImageBytes;
                                _unitOfWork.AttachAsModified(trainer);
                                await _unitOfWork.SaveChangesAsync();
                                Debug.WriteLine("Trainer entity updated and saved to database.");
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine($"Error saving image for Trainer: {ex.Message}");
                                throw;
                            }
                        }
                        else
                        {
                            throw new InvalidOperationException("Logged user is neither a Member nor a Trainer.");
                        }

                        // Update UI
                        Debug.WriteLine("Updating UI with the new image.");
                        await CoreApplication.MainView.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                        {
                            var bitmapImage = new BitmapImage();
                            using (var memoryStream = new MemoryStream(resizedImageBytes))
                            {
                                await bitmapImage.SetSourceAsync(memoryStream.AsRandomAccessStream());
                                ProfilePictureBitmap = bitmapImage;
                            }
                            Debug.WriteLine("UI updated successfully with new profile picture.");
                        });

                        Debug.WriteLine("Image uploaded, resized, saved locally, and updated in the UI.");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error during image upload process: {ex.Message}");
                }
            }
            else
            {
                Debug.WriteLine("No file selected.");
            }
        }


        public async Task LoadProfilePictureAsync()
        {
            try
            {
                // Determine the logged user's email
                string email = null;
                if (_userViewModel.LoggedUser is Member member)
                {
                    email = member.Email;
                }
                else if (_userViewModel.LoggedUser is Trainer trainer)
                {
                    email = trainer.Email;
                }

                if (email == null)
                {
                    throw new InvalidOperationException("Logged user does not have an email property.");
                }

                // Fetch the local file
                var folder = ApplicationData.Current.LocalFolder;
                var file = await folder.TryGetItemAsync($"{email}_ProfilePicture.jpg") as StorageFile;

                if (file != null)
                {
                    Debug.WriteLine($"Loading profile picture from local folder: {file.Path}");
                    var bitmapImage = new BitmapImage();
                    using (var fileStream = await file.OpenAsync(FileAccessMode.Read))
                    {
                        await bitmapImage.SetSourceAsync(fileStream);
                    }
                    ProfilePictureBitmap = bitmapImage;
                }
                else
                {
                    Debug.WriteLine("Profile picture not found locally.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading profile picture: {ex.Message}");
            }
        }


        private async Task LoadFromDatabaseAndCache(byte[] imageData, string localImagePath)
        {
            try
            {
                var bitmapImage = new BitmapImage();
                using (var memoryStream = new MemoryStream(imageData))
                {
                    await bitmapImage.SetSourceAsync(memoryStream.AsRandomAccessStream());
                }

                ProfilePictureBitmap = bitmapImage;

                // Save to the local folder for future use
                await File.WriteAllBytesAsync(localImagePath, imageData);
                Debug.WriteLine($"Profile picture cached locally: {localImagePath}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading and caching profile picture: {ex.Message}");
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
                        // Fetch average rating
                        AverageRating = await _unitOfWork.Reviews.GetAverageRatingByTrainerIdAsync(fetchedTrainer.Id);
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

            // Exclude weight and height for trainers
            if (user is Member)
            {
                Weight = user.Weight;
                Height = user.Height;
                RemainingWorkouts = user.RemainingWorkouts;
                Membership = user.Membership;
            }

            if (user is Trainer)
            {
                UserRole = Role.Trainer;
                RemainingWorkouts = 0; // Trainers do not have this property
            }
            
            if (user is Admin) UserRole = Role.Admin;
            
            // Convert byte[] to BitmapImage for ProfilePicture
            if (user.Image != null)
            {
                using (var imageStream = new MemoryStream(user.Image))
                {
                    var bitmapImage = new BitmapImage();
                    bitmapImage.SetSourceAsync(imageStream.AsRandomAccessStream()).AsTask().Wait();
                    ProfilePictureBitmap = bitmapImage;
                }
            }
            else
            {
                ProfilePictureBitmap = null; // No image, fallback to default behavior
            }

            OnPropertyChanged(nameof(IsMember));
            OnPropertyChanged(nameof(IsTrainer));
            OnPropertyChanged(nameof(IsAdmin));
            OnPropertyChanged(nameof(FullName));
            OnPropertyChanged(nameof(Email));
            OnPropertyChanged(nameof(Membership));
            OnPropertyChanged(nameof(IsProfileComplete));
            OnPropertyChanged(nameof(ProfilePictureBitmap));
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
                    existingMember.Image = member.Image; // Save byte array

                    // Attach the entity and mark it as modified
                    _unitOfWork.AttachAsModified(existingMember);
                }
                else if (_userViewModel.LoggedUser is Trainer trainer)
                {
                    var existingTrainer = await _unitOfWork.Trainers.GetTrainerByEmailAsync(trainer.Email);
                    if (existingTrainer == null)
                    {
                        Debug.WriteLine($"Trainer with email {trainer.Email} not found.");
                        return;
                    }

                    // Update trainer's properties
                    existingTrainer.FullName = FullName;
                    existingTrainer.Username = Username;
                    existingTrainer.Email = Email;
                    existingTrainer.PhoneNumber = PhoneNumber;

                    // Attach and save
                    _unitOfWork.AttachAsModified(existingTrainer);
                    await _unitOfWork.SaveChangesAsync();
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
