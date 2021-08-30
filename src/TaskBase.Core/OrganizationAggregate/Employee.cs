using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;
using System.Text;
using TaskBase.Core.Shared;

namespace TaskBase.Core.OrganizationAggregate
{
    public class Employee : Entity<Guid>
    {
        public Employee(Guid organizationId, string firstName, string lastName, string profileImageUrl, string department)
            : base(Guid.NewGuid())
        {
            OrganizationId = organizationId;
            FirstName = firstName;
            LastName = lastName;
            ProfileImageUrl = profileImageUrl;
            Department = department;
        }

        public Guid OrganizationId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfileImageUrl { get; set; }
        public string Department { get; set; }
    }
}
