namespace AuthReclame
{
    using Microsoft.AspNetCore.Authorization;

    class AgeRequirement : IAuthorizationRequirement
    {
        protected internal int Age { get; set; }
        public AgeRequirement(int age) => Age = age;
    }
}
