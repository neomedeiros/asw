using System;
using System.Threading.Tasks;
using ASW.Entities;
using ASW.Entities.Enums;
using ASW.Exceptions;
using ASW.Repositories.Contracts;
using ASW.Services;
using ASW.Tests.Factories;
using FluentAssertions;
using Moq;
using Xunit;

namespace ASW.Tests
{
    public class DiffServiceTest
    {
        public DiffServiceTest()
        {
            _comparisonRepositoryMock = new Mock<IDiffRepository>();
            _target = new DiffService(_comparisonRepositoryMock.Object);
        }

        private readonly DiffService _target;
        private readonly Mock<IDiffRepository> _comparisonRepositoryMock;

        [Fact]
        public void PostDiffEntry_WHEN_id_lower_or_equal_zero_SHOULD_throw_argument_out_of_range_exception()
        {
            Func<Task> postDiffEntry = async () => await _target.PostDiffEntry(-1, Side.Left, null);

            postDiffEntry.ShouldThrow<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void PostDiffEntry_WHEN_data_null_SHOULD_throw_argument_null_exception()
        {
            Func<Task> postDiffEntry = async () => await _target.PostDiffEntry(1, Side.Left, null);

            postDiffEntry.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public async Task PostDiffEntry_WHEN_request_is_new_SHOULD_call_repository_insert()
        {
            _comparisonRepositoryMock.Setup(m => m.Get(1)).ReturnsAsync(() => null);

            await _target.PostDiffEntry(1, Side.Left, "someValue");

            _comparisonRepositoryMock.Verify(repository => repository.Insert(It.IsAny<ComparisonRequestEntity>()), Times.Once);
        }

        [Fact]
        public async Task PostDiffEntry_WHEN_request_is_not_new_SHOULD_call_repository_update()
        {
            _comparisonRepositoryMock.Setup(m => m.Get(1)).ReturnsAsync(ComparisonRequestFactory.GetSingleSideNull(1, Side.Left, "someValue"));

            await _target.PostDiffEntry(1, Side.Left, "someValue");

            _comparisonRepositoryMock.Verify(repository => repository.Update(It.IsAny<ComparisonRequestEntity>()), Times.Once);
        }

        [Fact]
        public void Compare_WHEN_id_lower_or_equal_zero_SHOULD_throw_argument_out_of_range_exception()
        {
            Func<Task> compare = async () => await _target.Diff(-1);

            compare.ShouldThrow<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void Compare_WHEN_left_not_null_and_right_null_SHOULD_throw_insuficient_data_for_comparison_exception()
        {
            _comparisonRepositoryMock.Setup(m => m.Get(1))
                .ReturnsAsync(ComparisonRequestFactory.GetSingleSideNull(1, Side.Right, "someValue"));

            Func<Task> compare = async () => await _target.Diff(1);
            compare.ShouldThrow<InsuficientDataForComparisonException>();
        }

        [Fact]
        public void Compare_WHEN_left_null_and_right_not_null_SHOULD_throw_insuficient_data_for_comparison_exception()
        {
            _comparisonRepositoryMock.Setup(m => m.Get(1))
                .ReturnsAsync(ComparisonRequestFactory.GetSingleSideNull(1, Side.Left, "someValue"));

            Func<Task> compare = async () => await _target.Diff(1);
            compare.ShouldThrow<InsuficientDataForComparisonException>();
        }

        [Fact]
        public void Compare_WHEN_left_null_and_right_null_SHOULD_throw_insuficient_data_for_comparison_exception()
        {
            _comparisonRepositoryMock.Setup(m => m.Get(1))
                .ReturnsAsync(ComparisonRequestFactory.GetSingleSideNull(1, Side.Left, "someValue"));

            Func<Task> compare = async () => await _target.Diff(1);
            compare.ShouldThrow<InsuficientDataForComparisonException>();
        }

        [Fact]
        public async Task Compare_WHEN_left_and_right_are_empty_SHOULD_return_equal()
        {
            _comparisonRepositoryMock.Setup(m => m.Get(1))
                .ReturnsAsync(ComparisonRequestFactory.GetValid(1, string.Empty, string.Empty));

            var result = await _target.Diff(1);

            result.Id.Should().Be(1);
            result.Left.Should().Be(string.Empty);
            result.Right.Should().Be(string.Empty);
            result.AreEqual.Should().Be(true);
            result.HaveSameSize.Should().Be(true);
            result.DiffInsights.Should().BeEmpty();
        }

        [Fact]
        public async Task Compare_WHEN_left_and_right_different_all_SHOULD_return_that()
        {
            _comparisonRepositoryMock.Setup(m => m.Get(1))
                .ReturnsAsync(ComparisonRequestFactory.GetValid(1, "bcc", "baba"));

            var result = await _target.Diff(1);

            result.AreEqual.Should().Be(false);
            result.HaveSameSize.Should().Be(false);
            result.DiffInsights.Should().BeEmpty();
        }

        [Fact]
        public async Task Compare_WHEN_left_and_right_different_size_SHOULD_return_that()
        {
            _comparisonRepositoryMock.Setup(m => m.Get(1))
                .ReturnsAsync(ComparisonRequestFactory.GetValid(1, "aaa", "aaaaa"));

            var result = await _target.Diff(1);

            result.AreEqual.Should().Be(false);
            result.HaveSameSize.Should().Be(false);
            result.DiffInsights.Should().BeEmpty();
        }

        [Fact]
        public async Task Compare_WHEN_left_and_right_equals_SHOULD_return_that()
        {
            _comparisonRepositoryMock.Setup(m => m.Get(1))
                .ReturnsAsync(ComparisonRequestFactory.GetValid(1, "aaaaa", "aaaaa"));

            var result = await _target.Diff(1);

            result.Id.Should().Be(1);
            result.Left.Should().Be("aaaaa");
            result.Right.Should().Be("aaaaa");
            result.AreEqual.Should().Be(true);
            result.HaveSameSize.Should().Be(true);
            result.DiffInsights.Should().BeEmpty();
        }

        [Fact]
        public async Task Compare_WHEN_left_and_right_different_but_same_size_SHOULD_return_that()
        {
            _comparisonRepositoryMock.Setup(m => m.Get(1))
                .ReturnsAsync(ComparisonRequestFactory.GetValid(1, "bbbbb", "aaaaa"));

            var result = await _target.Diff(1);

            result.AreEqual.Should().Be(false);
            result.HaveSameSize.Should().Be(true);
            result.DiffInsights.Should().NotBeEmpty();
        }

        [Fact]
        public async Task Compare_WHEN_left_and_right_different_but_same_size_SHOULD_return_one_insight()
        {
            _comparisonRepositoryMock.Setup(m => m.Get(1))
                .ReturnsAsync(ComparisonRequestFactory.GetValid(1, "11110", "11111"));

            var result = await _target.Diff(1);

            result.AreEqual.Should().Be(false);
            result.HaveSameSize.Should().Be(true);
            result.DiffInsights.Should().HaveCount(1);
        }

        [Fact]
        public async Task Compare_WHEN_left_and_right_different_but_same_size_SHOULD_return_one_insights()
        {
            _comparisonRepositoryMock.Setup(m => m.Get(1))
                .ReturnsAsync(ComparisonRequestFactory.GetValid(1, "11110", "11111"));

            var result = await _target.Diff(1);

            result.AreEqual.Should().Be(false);
            result.HaveSameSize.Should().Be(true);
            result.DiffInsights.Should().HaveCount(1);
        }

        [Fact]
        public async Task Compare_WHEN_left_and_right_different_but_same_size_SHOULD_return_two_insights()
        {
            _comparisonRepositoryMock.Setup(m => m.Get(1))
                .ReturnsAsync(ComparisonRequestFactory.GetValid(1, "00000000000", "00123000450"));

            var result = await _target.Diff(1);

            result.AreEqual.Should().Be(false);
            result.HaveSameSize.Should().Be(true);
            result.DiffInsights.Should().HaveCount(2);
        }
    }
}