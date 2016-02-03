﻿using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Abp.EntityFramework;
using rooferlocator.com.Common;

namespace rooferlocator.com.EntityFramework.Repositories.Common
{
    public class TestimonialRepository : comRepositoryBase<Testimonial>, ITestimonialRepository
    {
        public TestimonialRepository(IDbContextProvider<comDbContext> dbContextProvider) : base(dbContextProvider) { }
    }
}
