using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ASW.Entities;
using ASW.Entities.Enums;
using ASW.Exceptions;
using ASW.Models;
using ASW.Repositories.Contracts;
using ASW.Services.Contracts;

namespace ASW.Services
{
    public class ComparisonService : IComparisonService
    {
        private readonly IComparisonRepository _comparisonRepository;

        public ComparisonService(IComparisonRepository comparisonRepository)
        {
            _comparisonRepository = comparisonRepository;
        }

        public async Task PostDiffEntry(long id, Side side, string data)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));
            if (data == null) throw new ArgumentNullException(nameof(data));

            var comparisonEntity = await _comparisonRepository.Get(id);

            if (comparisonEntity == null)
            {
                await InsertComparisonRequest(id, side, data);
                return;
            }

            UpdateComparisonRequest(side, data, comparisonEntity);
        }

        private void UpdateComparisonRequest(Side side, string data, ComparisonRequestEntity comparisonEntity)
        {
            comparisonEntity.Left = side == Side.Left ? data : comparisonEntity.Left;
            comparisonEntity.Right = side == Side.Right ? data : comparisonEntity.Right;

            _comparisonRepository.Update(comparisonEntity);
            _comparisonRepository.SaveChanges();
        }

        private async Task InsertComparisonRequest(long id, Side side, string data)
        {
            ComparisonRequestEntity comparisonEntity;
            comparisonEntity = new ComparisonRequestEntity
            {
                Id = id,
                Left = side == Side.Left ? data : null,
                Right = side == Side.Right ? data : null
            };

            await _comparisonRepository.Insert(comparisonEntity);
            await _comparisonRepository.SaveChangesAsync();
        }

        public async Task<DiffResultModel> Diff(long id)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));

            var comparisonEntity = await _comparisonRepository.Get(id);
            if (comparisonEntity == null)
                throw new ComparisonRequestNotFoundException();

            if (comparisonEntity.Left == null || comparisonEntity.Right == null)
                throw new InsuficientDataForComparisonException();

            return Diff(comparisonEntity);
        }

        private DiffResultModel Diff(ComparisonRequestEntity comparisonEntity)
        {
            var result = new DiffResultModel
            {
                Id = comparisonEntity.Id,
                Right = comparisonEntity.Right,
                Left = comparisonEntity.Left
            };

            if (comparisonEntity.Left.Equals(comparisonEntity.Right))
                result.AreEqual = true;

            result.HaveSameSize = comparisonEntity.Right.Length == comparisonEntity.Left.Length;

            if (!result.HaveSameSize) return result;

            result.DiffInsights = GetDiffInsights(result.Left, result.Right);

            return result;
        }

        private List<string> GetDiffInsights(string left, string right)
        {
            var result = new List<string>();

            for (var position = 0; position < left.Length; position++)
            {

                var leftDifferenceBuilder = new StringBuilder();
                var rightDifferenceBuilder = new StringBuilder();

                while (position < left.Length && left[position] != right[position])
                {
                    leftDifferenceBuilder.Append(left[position]);
                    rightDifferenceBuilder.Append(right[position]);

                    position++;
                }

                if (leftDifferenceBuilder.Length > 0)
                    result.Add($"Starting at offset {position}, the left data has the value '{leftDifferenceBuilder}', and right data has '{rightDifferenceBuilder}'.");
            }

            return result;
        }
    }
}