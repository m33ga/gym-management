using System;
using System.Text;
using GymManagement.Domain;
using GymManagement.Domain.Models;
using GymManagement.Application;
using System.Security.Cryptography;
using GymManagement.Infrastructure;
using GymManagement.ConsoleUtilities;
using GymManagement.Console.ConsoleUtilities;

Console.WriteLine("Testing GymManagement");

// Main method and loop:
await MainMenu.ShowMainMenuAsync();
