using Microsoft.AspNetCore.Mvc.Rendering;

namespace EShop.Common;

public static class CommonExtensionMethods
{
    public static List<SelectListItem> CreateSelectListItem<T>(
        this List<T> items,
        object selectedItem = null,
        bool addChooseOneItem = true,
        string firstItemText = "انتخاب کنید",
        string firstItemValue = "0"
    )
    {
        var result = new List<SelectListItem>();
        if (addChooseOneItem)
            result.Add(new SelectListItem(firstItemText, firstItemValue));
        if (items.Any())
        {
            var modelType = items.First().GetType();

            var idProperty = modelType.GetProperty("Id");
            var titleProperty = modelType.GetProperty("Title");
            if (idProperty is null || titleProperty is null)
                throw new ArgumentNullException(
                    $"{typeof(T).Name} must have ```Id``` and ```Title``` propeties");
            foreach (var item in items)
            {
                var id = idProperty.GetValue(item)?.ToString();
                var text = titleProperty.GetValue(item)?.ToString();
                var selected = selectedItem?.ToString() == id;
                result.Add(new SelectListItem(text, id, selected));
            }
        }

        return result;
    }
}
