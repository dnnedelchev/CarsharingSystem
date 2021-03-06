﻿using System;
using System.Web.Http;
using CarsharingSystem.Model;
using CarsharingSystem.Data;

namespace CarsharingSystem.WebServices.Controllers
{
    using CarsharingSystem.WebServices.Models;

    using Microsoft.AspNet.Identity;

    [RoutePrefix("api")]
    public class DriverController : ApiController
    {
        private readonly ICarsharingData db;

        public DriverController()
            : this(new CarsharingData())
        {   
        }

        public DriverController(ICarsharingData data)
        {
            this.db = data;
        }

        // POST api/Driver/DrivingLicense
        [Authorize]
        [HttpPost]
        [Route("Drivers/DrivingLicense")]
        public IHttpActionResult PostDriverDrivingLicense(DrivingLicenseInputModel inputModel)
        {
            if (inputModel == null)
            {
                return this.BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }

            var currentDriverId = User.Identity.GetUserId();
            var user = this.db.Drivers.Find(currentDriverId);

            if (user == null)
            {
                return this.BadRequest();
            }


            if (!string.IsNullOrWhiteSpace(inputModel.Categories))
            {
                var allCategories = inputModel.Categories.Split(',');
                foreach (var category in allCategories)
                {
                    Category cat;
                    if (!Enum.TryParse(category, out cat))
                    {
                        return this.BadRequest();
                    }
                }
            }

            var drivingLicenseToBeAdded = new DrivingLicense
                {
                    LicenseNumber = inputModel.LicenseNumber,
                    ExpiryDate = inputModel.ExpiryDate,
                    DriverId = currentDriverId,
                    Driver = user,
                    DrivingLicenseCategories = inputModel.Categories
                };

            this.db.DrivingLicenses.Add(drivingLicenseToBeAdded);
            this.db.SaveChanges();

            user.DrivingLicenseId = drivingLicenseToBeAdded.Id;
            this.db.SaveChanges();

            return this.CreatedAtRoute(
                    "DefaultApi",
                    new { controller = "DrivingLicense", id = drivingLicenseToBeAdded.Id },
                    new
                    {
                        Id = drivingLicenseToBeAdded.Id,
                        Message = "Driving licesed was succesfully added."
                    }
                    );
        }

        // POST api/Drivers/Vehicle
        [Authorize]
        [HttpPost]
        [Route("Drivers/Vehicle")]
        public IHttpActionResult PostDriverVehicle(VehicleInputModel inputModel)
        {
            if (inputModel == null)
            {
                return this.BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var currentDriverId = this.User.Identity.GetUserId();
            var driver = this.db.Drivers.Find(currentDriverId);

            var vehicleToBeAdded = new Vehicle
                                       {
                                           DriverId = currentDriverId,
                                           ManufactureDate = inputModel.ManufactureDate.Value,
                                           VehicleType = inputModel.VehicleType.Value,
                                           Seats = inputModel.Seats.Value,
                                           Run = inputModel.Run.Value
                                       };

            driver.Vehicles.Add(vehicleToBeAdded);

            this.db.SaveChanges();

            return this.CreatedAtRoute(
                "DefaultApi",
                new { controller = "Vehicles", id = vehicleToBeAdded.Id },
                new { Id = vehicleToBeAdded.Id, Message = "Vehicle was succesfully added." });
        }

        //    // GET api/Driver/5
    //    [ResponseType(typeof(Driver))]
    //    public IHttpActionResult GetDriver(string id)
    //    {
    //        Driver driver = db.Users.Find(id);
    //        if (driver == null)
    //        {
    //            return NotFound();
    //        }

    //        return Ok(driver);
    //    }

    //    // PUT api/Driver/5
    //    public IHttpActionResult PutDriver(string id, Driver driver)
    //    {
    //        if (!ModelState.IsValid)
    //        {
    //            return BadRequest(ModelState);
    //        }

    //        if (id != driver.Id)
    //        {
    //            return BadRequest();
    //        }

    //        db.Entry(driver).State = EntityState.Modified;

    //        try
    //        {
    //            db.SaveChanges();
    //        }
    //        catch (DbUpdateConcurrencyException)
    //        {
    //            if (!DriverExists(id))
    //            {
    //                return NotFound();
    //            }
    //            else
    //            {
    //                throw;
    //            }
    //        }

    //        return StatusCode(HttpStatusCode.NoContent);
    //    }

    //    // POST api/Driver
    //    [ResponseType(typeof(Driver))]
    //    public IHttpActionResult PostDriver(Driver driver)
    //    {
    //        if (!ModelState.IsValid)
    //        {
    //            return BadRequest(ModelState);
    //        }

    //        db.Users.Add(driver);

    //        try
    //        {
    //            db.SaveChanges();
    //        }
    //        catch (DbUpdateException)
    //        {
    //            if (DriverExists(driver.Id))
    //            {
    //                return Conflict();
    //            }
    //            else
    //            {
    //                throw;
    //            }
    //        }

    //        return CreatedAtRoute("DefaultApi", new { id = driver.Id }, driver);
    //    }

    //    // DELETE api/Driver/5
    //    [ResponseType(typeof(Driver))]
    //    public IHttpActionResult DeleteDriver(string id)
    //    {
    //        Driver driver = db.Users.Find(id);
    //        if (driver == null)
    //        {
    //            return NotFound();
    //        }

    //        db.Users.Remove(driver);
    //        db.SaveChanges();

    //        return Ok(driver);
    //    }

    //    protected override void Dispose(bool disposing)
    //    {
    //        if (disposing)
    //        {
    //            db.Dispose();
    //        }
    //        base.Dispose(disposing);
    //    }

    //    private bool DriverExists(string id)
    //    {
    //        return db.Users.Count(e => e.Id == id) > 0;
    //    }
    }
}