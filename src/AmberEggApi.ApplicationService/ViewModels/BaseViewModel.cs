﻿using Api.Common.Contracts.Entities;
using System;

namespace AmberEggApi.ApplicationService.ViewModels
{
    public abstract class BaseViewModel : IBaseViewModel
    {
        public Guid Id { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime? ModifiedDate { get; set; }
        public int Version { get; set; }
    }
}