// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI.Collections;

namespace CollectionsExperiment.Samples;

[ToolkitSample(id: nameof(IncrementalLoadingCollectionSample), "Incremental Loading Collection", description: $"A sample for showing how to create and use a IncrementalLoadingCollection.")]
public sealed partial class IncrementalLoadingCollectionSample : Page
{
    // IncrementalLoadingCollection can be bound to a GridView or a ListView. In this case it is a ListView called PeopleListView.
    public IncrementalLoadingCollection<PeopleSource, Person> PeopleSource { get; set; } = new(new PeopleSource());

    public IncrementalLoadingCollectionSample()
    {
        this.InitializeComponent();
    }
}

/// <summary>
/// A sample implementation of the <see cref="IIncrementalSource{TSource}"/> interface.
/// </summary>
/// <seealso cref="IIncrementalSource{TSource}"/>
public partial class PeopleSource : IIncrementalSource<Person>
{
    private readonly List<Person> _people;

    /// <summary>
    /// Initializes a new instance of the <see cref="PeopleSource"/> class.
    /// </summary>
    public PeopleSource()
    {
        // Creates an example collection.
        _people = new List<Person>();

        for (int i = 1; i <= 200; i++)
        {
            var p = new Person { Name = "Person " + i };
            _people.Add(p);
        }
    }

    /// <summary>
    /// Retrieves items based on <paramref name="pageIndex"/> and <paramref name="pageSize"/> arguments.
    /// </summary>
    /// <param name="pageIndex">
    /// The zero-based index of the page that corresponds to the items to retrieve.
    /// </param>
    /// <param name="pageSize">
    /// The number of <see cref="Person"/> items to retrieve for the specified <paramref name="pageIndex"/>.
    /// </param>
    /// <param name="cancellationToken">
    /// Used to propagate notification that operation should be canceled.
    /// </param>
    /// <returns>
    /// Returns a collection of <see cref="Person"/>.
    /// </returns>
    public async Task<IEnumerable<Person>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default(CancellationToken))
    {
        // Gets items from the collection according to pageIndex and pageSize parameters.
        var result = (from p in _people
                      select p).Skip(pageIndex * pageSize).Take(pageSize);

        // Simulates a longer request...
        // Make sure the list is still in order after a refresh,
        // even if the first page takes longer to load
        if (pageIndex == 0)
        {
            await Task.Delay(2000);
        }
        else
        {
            await Task.Delay(1000);
        }

        return result;
    }
}

/// <summary>
/// A sample class used to show how to use the <see cref="IIncrementalSource{TSource}"/> interface.
/// </summary>
public partial class Person
{
    /// <summary>
    /// Gets or sets the name of the person.
    /// </summary>
    public string? Name { get; set; }
}
