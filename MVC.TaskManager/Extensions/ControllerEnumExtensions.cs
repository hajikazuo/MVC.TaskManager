using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;

namespace MVC.TaskManager.Extensions
{
    public static class ControllerEnumExtensions
    {
        public static List<SelectListItem> AssembleSelectListToEnum<T>(this Controller controller, T selected = default(T), bool excludeDefault = false) where T : struct, IConvertible
        {
            var excludeCallback = default(Func<T, bool>);
            if (excludeDefault)
            {
                excludeCallback = (enumerator) => enumerator.Equals(default(T));
            }
            return controller.AssembleSelectListToEnum(selected, excludeCallback);
        }

        public static List<SelectListItem> AssembleSelectListToEnum<T>(this Controller controller, T selected, Func<T, bool> excludeCallback) where T : struct, IConvertible
        {
            var items = new List<SelectListItem>();
            var enums = Enum.GetValues(typeof(T)).Cast<T>();
            foreach (var enumerator in enums.OrderBy(o => o.GetDisplayName()))
            {
                if (excludeCallback != default && excludeCallback(enumerator))
                    continue;
                var name = enumerator.GetDisplayName();
                var item = new SelectListItem()
                {
                    Value = enumerator.ToString(),
                    Text = name,
                    Selected = selected.Equals(enumerator)
                };
                items.Add(item);
            }
            return items;
        }
    }
}
