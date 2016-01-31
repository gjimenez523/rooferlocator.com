using Abp.Web.Mvc.Views;

namespace rooferlocator.com.Web.Views
{
    public abstract class comWebViewPageBase : comWebViewPageBase<dynamic>
    {

    }

    public abstract class comWebViewPageBase<TModel> : AbpWebViewPage<TModel>
    {
        protected comWebViewPageBase()
        {
            LocalizationSourceName = comConsts.LocalizationSourceName;
        }
    }
}