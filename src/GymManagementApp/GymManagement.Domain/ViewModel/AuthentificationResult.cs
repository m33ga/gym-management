using GymManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagement.Domain.ViewModel
{
    public class AuthentificationResult
    {
        public bool IsAuthentificated { get; set; }
        public Role UserRole { get; set; }
    }
}
