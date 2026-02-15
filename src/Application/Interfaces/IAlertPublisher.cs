using Application.Metrics.Models;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface IAlertPublisher
    {
        Task PublishAlertAsync(Alert Alert, CancellationToken ct = default);
    }
}
