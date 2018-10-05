using System;
using System.Threading.Tasks;
using Actio.Common.Exceptions;
using Actio.Services.Activities.Domain.Models;
using Actio.Services.Activities.Domain.Repositories;

namespace Actio.Services.Activities.Services
{

    //For business logic
    public class ActivityService : IActivityService
    {
        private readonly IActivityRepo _activityRepository;
        private readonly ICategoryRepo _categoryRepository;

        public ActivityService(IActivityRepo activityRepository,
            ICategoryRepo categoryRepository)
        {
            _activityRepository = activityRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task AddAsync(Guid id, Guid userId, string category, string name, string description, DateTime createdAt)
        {
            var activityCategory = await _categoryRepository.GetAsync(category);
            if (activityCategory == null)
            {
                throw new ActioException("category_not_found", $"Category: {category} was not found");
            }

            var activity = new Activity(id, name, activityCategory, description, userId, createdAt);

            await _activityRepository.AddAsync(activity);
        }
    }
}
