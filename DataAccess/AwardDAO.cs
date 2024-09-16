using Charity_Management_System;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class AwardDAO : SingleOnBase<AwardDAO>
    {
        // Get all awards
        public async Task<IEnumerable<Award>> GetAllAwards()
        {
             return await _context.Awards.ToListAsync();
        }
        // Find award by id
        public async Task<Award> GetAwardById(int id)
        {
            _context = new DBContext();
            var awardID = await _context.Awards.FirstOrDefaultAsync(a => a.awardID == id);
            if (awardID == null)
            {
                return null;
            }
            return awardID;
        }
        // Add award
        public async Task AddAward(Award award)
        {
            _context.Awards.Add(award);
            await _context.SaveChangesAsync();
        }
        // Update award
        public async Task UpdateAward(Award award)
        {
            var existingItem = await GetAwardById(award.awardID);
            if (existingItem == null)
            {
                return;
            }
            _context.Awards.Update(award);
            await _context.SaveChangesAsync();
        }
        // Delete award
        public async Task DeleteAward(int id)
        {
            var award = await GetAwardById(id);
            if (award != null)
            {
                _context.Awards.Remove(award);
                await _context.SaveChangesAsync();
            }
        }
    }
}
