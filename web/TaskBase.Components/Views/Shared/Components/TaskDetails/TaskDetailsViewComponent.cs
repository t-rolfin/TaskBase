using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBase.Application.Models;
using TaskBase.Application.Queries.GetTask;
using TaskBase.Application.Services;
using TaskBase.Components.Models;
using TaskBase.Core.Enums;
using TaskBase.Core.TaskAggregate;

namespace TaskBase.Components.Views.Shared.Components.TaskDetails
{
    [ViewComponent(Name = "TaskDetails")]
    public class TaskDetailsViewComponent : ViewComponent
    {
        readonly IMediator _mediator;

        public TaskDetailsViewComponent(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IViewComponentResult> InvokeAsync(Guid TaskId)
        {
            if (TaskId == Guid.Empty)
                return View(TaskModel.EmptyModel);

            GetTaskQuery query = new(TaskId);
            var result = await _mediator.Send(query);

            return View(result.IsSuccess ? result.Value : TaskModel.EmptyModel);
        }
    }
}
