using GymManagement.Domain.Enums;
using GymManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagement.Domain.ViewModel
{
    public class RegistrationResult
    {
        public bool IsRegistered { get; set; }
        public Trainer Trainer { get; set; }
        public Member Member { get; set; }
        
    }
}
