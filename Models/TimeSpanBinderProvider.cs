using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace WebQuanLyNhaKhoa.Models
{
    public class TimeSpanBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Metadata.ModelType == typeof(TimeSpan))
            {
                return new BinderTypeModelBinder(typeof(TimeSpanBinder));
            }

            return null;
        }
    }



}
