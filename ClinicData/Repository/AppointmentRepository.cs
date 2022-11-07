using ClinicData.Data;
using ClinicEntity.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClinicData.Repository
{
    public class AppointmentRepository: IAppointmentRepository
    {
        ClinicDbContext _appointmentDbContext;


        public AppointmentRepository(ClinicDbContext appointmentDbContext)
        {
            _appointmentDbContext = appointmentDbContext;
        }

        public void AddAppointment(Appointment appointment)
        {
            object value = _appointmentDbContext.Add(appointment);
            _appointmentDbContext.SaveChanges();
        }

        public void UpdateAppointment(Appointment appointment)
        {
            _appointmentDbContext.Entry(appointment).State = EntityState.Modified;
            _appointmentDbContext.SaveChanges();
        }

        public IEnumerable<Appointment> GetAppointment()
        {
            return _appointmentDbContext.appointments.ToList();
        }

        public void DeleteAppointment(int AppointID)
        {
            var appointments = _appointmentDbContext.appointments.Find(AppointID);
            _appointmentDbContext.appointments.Remove(appointments);
            _appointmentDbContext.SaveChanges();
        }
    }
}
