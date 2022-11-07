using ClinicData.Repository;
using ClinicEntity.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicBusiness.Services
{
    public class AppointmentService
    {
        IAppointmentRepository _appointmentRepository;
        public AppointmentService(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        //Add Appointment
        public void AddAppointment(Appointment appointment)
        {
            _appointmentRepository.AddAppointment(appointment);
        }

        //Delete Appointment

        public void DeleteAppointment(int AppointID)
        {
            _appointmentRepository.DeleteAppointment(AppointID);
        }

        //Update Appointment

        public void UpdateAppointment(Appointment appointment)
        {
            _appointmentRepository.UpdateAppointment(appointment);
        }

        //Get getAppointments

        public IEnumerable<Appointment> GetAppointment()
        {
            return _appointmentRepository.GetAppointment();
        }
    }
}
