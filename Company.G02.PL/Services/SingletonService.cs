﻿namespace Company.G02.PL.Services
{
    public class SingletonService : ISingletonService
    {
        public SingletonService()
        {
            Guid = Guid.NewGuid();
        }
        public Guid Guid { get; set; }

        public string GetGuid()
        {
            return Guid.ToString();
        }
    }
}
