using AutoMapper;
using TaskManagerAPI.DTOs;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Helpers
{
    /// <summary>
    /// Configures AutoMapper mappings.
    /// </summary>
    public class TaskProfile : Profile
    {
        public TaskProfile()
        {
            CreateMap<TaskCreateDto, TaskItem>();
            CreateMap<TaskUpdateDto, TaskItem>();
            CreateMap<TaskItem, TaskResponseDto>();
        }
    }
}
