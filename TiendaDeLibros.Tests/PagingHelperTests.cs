using TiendaDeLibros.Helpers;
using Xunit;

namespace TiendaDeLibros.Tests;

public class PagingHelperTests
{
    [Fact]
    public void ToPagedResponse_ReturnsFirstPage_WithDefaultSize()
    {
        var source = Enumerable.Range(1, 25).ToList();

        var page = PagingHelper.ToPagedResponse(source, pageNumber: 1, pageSize: 10);

        Assert.Equal(10, page.Items.Count);
        Assert.Equal([1, 2, 3, 4, 5, 6, 7, 8, 9, 10], page.Items);
        Assert.Equal(25, page.TotalCount);
        Assert.Equal(3, page.TotalPages);
    }

    [Fact]
    public void ToPagedResponse_ReturnsPartialLastPage()
    {
        var source = Enumerable.Range(1, 25).ToList();

        var page = PagingHelper.ToPagedResponse(source, pageNumber: 3, pageSize: 10);

        Assert.Equal(5, page.Items.Count);
        Assert.Equal([21, 22, 23, 24, 25], page.Items);
    }

    [Fact]
    public void ToPagedResponse_ClampsPageNumberBelowOne()
    {
        var source = Enumerable.Range(1, 5).ToList();

        var page = PagingHelper.ToPagedResponse(source, pageNumber: 0, pageSize: 10);

        Assert.Equal(1, page.PageNumber);
    }

    [Fact]
    public void ToPagedResponse_ClampsPageSizeAboveMax()
    {
        var source = Enumerable.Range(1, 200).ToList();

        var page = PagingHelper.ToPagedResponse(source, pageNumber: 1, pageSize: 1000);

        Assert.Equal(PagingHelper.MaxPageSize, page.PageSize);
    }

    [Fact]
    public void ToPagedResponse_ReturnsEmptyItems_WhenPageIsBeyondRange()
    {
        var source = Enumerable.Range(1, 5).ToList();

        var page = PagingHelper.ToPagedResponse(source, pageNumber: 5, pageSize: 10);

        Assert.Empty(page.Items);
    }
}
