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
    public class DiffService : IDiffService
    {
        private readonly IDiffRepository _diffRepository;

        public DiffService(IDiffRepository diffRepository)
        {
            _diffRepository = diffRepository;
        }

        public async Task PostDiffEntry(long id, Side side, string data)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));
            if (data == null) throw new ArgumentNullException(nameof(data));

            var comparisonEntity = await _diffRepository.Get(id);

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

            _diffRepository.Update(comparisonEntity);
            _diffRepository.SaveChanges();
        }

        private async Task InsertComparisonRequest(long id, Side side, string data)
        {
            var comparisonEntity = new ComparisonRequestEntity
            {
                Id = id,
                Left = side == Side.Left ? data : null,
                Right = side == Side.Right ? data : null
            };

            await _diffRepository.Insert(comparisonEntity);
            await _diffRepository.SaveChangesAsync();
        }

        public async Task<DiffResultModel> Diff(long id)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));

            var comparisonEntity = await _diffRepository.Get(id);
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

            if (comparisonEntity.Left.Equals(comparisonEntity.Right)) result.AreEqual = true;

            result.HaveSameSize = comparisonEntity.Right.Length == comparisonEntity.Left.Length;

            if (!result.HaveSameSize) return result;

            result.DiffInsights = GetDiffInsights(result.Left, result.Right);

            return result;
        }

        private List<string> GetDiffInsights(string left, string right)
        {
            var result = new List<string>();
            var offset = 0;
            var diffLength = 0;
            var diffOffset = 0;

            //Iterate over all characters of both sides, at same time
            while (offset < left.Length)
            {
                if (left[offset] != right[offset])
                {
                    //if chars are not equal, keep the first position of difference
                    if (diffOffset == 0) diffOffset = offset; 
                    
                    //start to count difference size
                    diffLength++;
                }
                else
                {
                    //at this position, data are equal (again). Create an insight if some difference was found
                    if (diffLength > 0)
                        result.Add($"Difference detected, starting at offset {diffOffset + 1} with length of {diffLength}.");

                    //reset difference variables to keep searching
                    diffLength = 0;
                    diffOffset = 0;
                }

                offset++;
            }

            //the last char might be different. If it is, create the last insight
            if (diffLength > 0)
                result.Add($"Difference detected, starting at offset {diffOffset + 1} with length of {diffLength}.");

            return result;
        }
    }
}