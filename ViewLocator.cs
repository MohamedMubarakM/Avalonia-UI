using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;

namespace Avalonia1
{

    public sealed class ViewLocator : IDataTemplate
    {

        
        public bool SupportsRecycling => true;

        public Control? Build(object? data)
        {
            if (data is null) return new TextBlock { Text = "null view model" };

            var name = data.GetType().FullName?.Replace("ViewModel", "View");
            if (string.IsNullOrWhiteSpace(name)) return new TextBlock { Text = "invalid view model" };

            var type = Type.GetType(name);
            if (type is null) return new TextBlock { Text = $"Not found: {name}" };

            return (Control)Activator.CreateInstance(type)!;
        }

        public bool Match(object? data) => data is not null;
    }
}
