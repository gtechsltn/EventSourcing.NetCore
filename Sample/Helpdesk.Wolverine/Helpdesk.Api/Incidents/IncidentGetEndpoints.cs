using Helpdesk.Api.Incidents.GetCustomerIncidentsSummary;
using Helpdesk.Api.Incidents.GetIncidentDetails;
using Helpdesk.Api.Incidents.GetIncidentHistory;
using Helpdesk.Api.Incidents.GetIncidentShortInfo;
using Marten;
using Marten.AspNetCore;
using Marten.Pagination;
using Microsoft.AspNetCore.Mvc;
using Wolverine.Http;

namespace Helpdesk.Api.Incidents;

public class IncidentGetEndpoints
{
    [WolverineGet("/api/customers/{customerId:guid}/incidents/")]
    public Task<IPagedList<IncidentShortInfo>> GetCustomerIncidents
    (IQuerySession querySession, Guid customerId, [FromQuery] int? pageNumber, [FromQuery] int? pageSize,
        CancellationToken ct) =>
        querySession.Query<IncidentShortInfo>().Where(i => i.CustomerId == customerId)
            .ToPagedListAsync(pageNumber ?? 1, pageSize ?? 10, ct);

    // That for some reason doesn't work for me
    // [WolverineGet("/api/incidents/{incidentId:guid}")]
    // public Task GetIncidentById([FromQuery] Guid incidentId, IQuerySession querySession, HttpContext context) =>
    //     querySession.Json.WriteById<IncidentDetails>(incidentId, context);

    [WolverineGet("/api/incidents/{incidentId:guid}")]
    public Task<IncidentDetails?> GetIncidentById([FromQuery] Guid incidentId, IQuerySession querySession,
        CancellationToken ct) =>
        querySession.LoadAsync<IncidentDetails>(incidentId, ct);

    // That for some reason doesn't work for me
    // [WolverineGet("/api/incidents/{incidentId:guid}/history")]
    // public Task GetIncidentHistory([FromQuery]Guid incidentId, HttpContext context, IQuerySession querySession) =>
    //     querySession.Query<IncidentHistory>().Where(i => i.IncidentId == incidentId).WriteArray(context);

    [WolverineGet("/api/incidents/{incidentId:guid}/history")]
    public Task<IReadOnlyList<IncidentHistory>> GetIncidentHistory(
        [FromQuery] Guid incidentId,
        IQuerySession querySession,
        CancellationToken ct
    ) =>
        querySession.Query<IncidentHistory>().Where(i => i.IncidentId == incidentId).ToListAsync(ct);

    // That for some reason doesn't work for me
    // [WolverineGet("/api/customers/{customerId:guid}/incidents/incidents-summary")]
    // public Task GetCustomerIncidentsSummary([FromQuery] Guid customerId, HttpContext context,
    //     IQuerySession querySession) =>
    //     querySession.Json.WriteById<CustomerIncidentsSummary>(customerId, context);

    [WolverineGet("/api/customers/{customerId:guid}/incidents/incidents-summary")]
    public Task GetCustomerIncidentsSummary(
        [FromQuery] Guid customerId,
        IQuerySession querySession,
        CancellationToken ct
    ) =>
        querySession.LoadAsync<CustomerIncidentsSummary>(customerId, ct);
}
