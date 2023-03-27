using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace ExampleApp.Identity.Store
{
    public partial class UserStore
    {
        public ILookupNormalizer Normalizer { get; set; }

        public UserStore(ILookupNormalizer normalizer)
        {
            Normalizer = normalizer;
            SeedStore();
        }

        private void SeedStore()
        {
            var customData = new Dictionary<string, (string food, string hobby)>
            {
                { "Alice", ("Pizza", "Running") },
                { "Bob", ("Ice Cream", "Cinema") },
                { "Charlie", ("Burgers", "Cooking") }
            };

            int idCounter = 0;

            string EmailFromName(string name) => $"{name.ToLower()}@example.com";

            foreach (string name in UsersAndClaims.Users)
            {
                AppUser user = new AppUser
                {
                    Id = (++idCounter).ToString(),
                    UserName = name,
                    NormalizedUserName = Normalizer.NormalizeName(name)
                };
                users.TryAdd(user.Id, user);
            }
        }
    }
}
