using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using reservation_system_be.Data;
using reservation_system_be.DTOs;
using reservation_system_be.Migrations;
using reservation_system_be.Models;
using reservation_system_be.Services.FileServices;

namespace reservation_system_be.Services.VehicleMakeServices
{
    public class VehicleMakeService : IVehicleMakeService
    {
        private const string AzureContainerName = "logos";
        private readonly DataContext _context;
        private readonly IFileService _fileServices;
        public VehicleMakeService(DataContext context, IFileService fileService)
        {
            _context = context; 
            _fileServices = fileService;
        }

        public async Task<List<VehicleMake>> GetAllVehicleMakes()
        {
            return await _context.VehicleMake.ToListAsync();
        }

        public async Task<VehicleMake> GetVehicleMake(int id)
        {
            var vehicleMake = await _context.VehicleMake.FindAsync(id);
            if (vehicleMake == null)
            {
                throw new DataNotFoundException("Vehicle make not found");
            }
            return vehicleMake;
        }

        public async Task<VehicleMake> CreateVehicleMake([FromForm] VehicleMakeDto vehicleMakeDto, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new DataNotFoundException("File not found");
            }

            // upload file to the azure storage and get link
            var fileUrl = await _fileServices.Upload(file, AzureContainerName);

            var vehicleMake = new VehicleMake
            {
                Name = vehicleMakeDto.Name,
                Logo = fileUrl
            };

            _context.VehicleMake.Add(vehicleMake);
            await _context.SaveChangesAsync();
            return vehicleMake;
        }

        public async Task<VehicleMake> UpdateVehicleMake(int id, [FromForm] VehicleMakeDto vehicleMakeDto, IFormFile file)
        {
            var existingVehicleMake = await _context.VehicleMake.FindAsync(id);
            if (existingVehicleMake == null)
            {
                throw new DataNotFoundException("Vehicle make not found");
            }

            if (file != null && file.Length > 0)
            {
                if (!string.IsNullOrEmpty(existingVehicleMake.Logo))
                {
                    await _fileServices.Delete(existingVehicleMake.Logo, AzureContainerName);
                }
                var fileUrl = await _fileServices.Upload(file, AzureContainerName);
                existingVehicleMake.Logo = fileUrl;
            }

            existingVehicleMake.Name = vehicleMakeDto.Name;
            _context.Entry(existingVehicleMake).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return existingVehicleMake;
        }


        public async Task DeleteVehicleMake(int id)
        {
            var vehicleMake = await _context.VehicleMake.FindAsync(id);
            if (vehicleMake == null)
            {
                throw new DataNotFoundException("Vehicle make not found");
            }

            if (!string.IsNullOrEmpty(vehicleMake.Logo))
            {
                await _fileServices.Delete(vehicleMake.Logo, AzureContainerName);
            }

            _context.VehicleMake.Remove(vehicleMake);
            await _context.SaveChangesAsync();
        }

    }
}