using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskBase.Core.Exceptions;
using TaskBase.Core.Interfaces;
using TaskBase.Core.Shared;
using Ardalis.GuardClauses;

namespace TaskBase.Core.OrganizationAggregate
{
    public class Organization : Entity<Guid>, IRootAggregate
    {
        private List<Employee> _employee;

        public Organization(string organizationName, List<Employee> employee = null)
            : base(Guid.NewGuid())
        {
            _employee = employee ?? new();
            this.OrganizationName = Guard.Against.NullOrWhiteSpace(organizationName, nameof(organizationName),
                "The name of organization must be provided!");
        }

        public string OrganizationName { get; set; }
        public IReadOnlyList<Employee> Employees
            => _employee.AsReadOnly();

        public void EnrollEmployee(Employee newEmployee)
        {
            if (newEmployee is null)
                throw new ArgumentNullException("An employee must be provided.");
            else
                _employee.Add(newEmployee);
        }

        public void DelistEmployee(Guid emplyeeId)
        {
            var employee = _employee.FirstOrDefault(x => x.Id == emplyeeId);

            if (employee != default)
                _employee.Remove(employee);
            else
                throw new EmployeeIsNotInOrganizationException(
                        "The specified employee wasn't found in this organization."
                    );
        }
    }
}
