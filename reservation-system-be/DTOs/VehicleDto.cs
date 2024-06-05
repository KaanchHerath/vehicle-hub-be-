﻿using reservation_system_be.Models;

namespace reservation_system_be.DTOs
{
    public record struct VehicleDto
    (
        int Id,
        string RegistrationNumber,
        string ChassisNo,
        string Colour,
        int Mileage,
        float CostPerDay,
        float CostPerExtraKM,
        string Transmission,
        bool Status,
        VehicleType VehicleType,
        VehicleModelDto VehicleModel,
        Employee Employee
    );
}