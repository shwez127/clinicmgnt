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
            _appointmentDbContext.appointments.Add(appointment);
            _appointmentDbContext.SaveChanges();
        }

        public void UpdateAppointment(Appointment appointment)
        {
            _appointmentDbContext.Entry(appointment).State = EntityState.Modified;
            _appointmentDbContext.SaveChanges();
        }

        public IEnumerable<Appointment> GetAppointment()
        {
            return _appointmentDbContext.appointments.Include(obj=>obj.Doctor).Include(obj=>obj.Patient).Include(obj => obj.Doctor.LoginTable).Include(obj => obj.Patient.LoginTable).ToList();
        }

        public void DeleteAppointment(int AppointID)
        {
            var appointments = _appointmentDbContext.appointments.Find(AppointID);
            _appointmentDbContext.appointments.Remove(appointments);
            _appointmentDbContext.SaveChanges();
        }
       

        public Appointment GetAppointmentById(int AppointID)
        {
            var result = _appointmentDbContext.appointments.Include(obj=>obj.Patient).Include(obj => obj.Doctor).Include(obj => obj.Doctor.Department).ToList();
            foreach (var appointment in result)
            {
                if(AppointID== appointment.AppointID)
                {
                    return appointment;
                }
            }
            return null;

        }
    }
}
