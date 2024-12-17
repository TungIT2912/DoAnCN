using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WebQuanLyNhaKhoa.Models
{
    //public class TimeSpanBinder : IModelBinder
    //{
    //    public Task BindModelAsync(ModelBindingContext bindingContext)
    //    {
    //        var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).FirstValue;

    //        if (string.IsNullOrEmpty(value))
    //        {
    //            return Task.CompletedTask;
    //        }

    //        if (TimeSpan.TryParse(value, out var time))
    //        {
    //            bindingContext.Result = ModelBindingResult.Success(time);
    //        }
    //        else
    //        {
    //            bindingContext.ModelState.AddModelError(bindingContext.ModelName, "Invalid time format");
    //        }

    //        return Task.CompletedTask;
    //    }
    //}
    public class TimeSpanBinder : IModelBinder
    {
        private readonly ILogger<TimeSpanBinder> _logger;

        public TimeSpanBinder(ILogger<TimeSpanBinder> logger)
        {
            _logger = logger;
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).FirstValue;
            _logger.LogInformation($"Attempting to bind TimeSpan for value: {value}");

            if (TimeSpan.TryParse(value, out var timeSpan))
            {
                bindingContext.Result = ModelBindingResult.Success(timeSpan);
            }
            else
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, "Invalid time format.");
            }
            return Task.CompletedTask;
        }
    }

}
