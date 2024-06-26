﻿using BarelySliced.Persistence;

using MediatR;

namespace BarelySliced.Business.Features.MyFeature
{
    public class MyFeatureHandler: IRequestHandler<MyFeatureRequest, MyFeatureResponse>
    {
        private readonly SliverDbContext db;

        public MyFeatureHandler(SliverDbContext db)
        {
            this.db = db;
        }
        public Task<MyFeatureResponse> Handle(MyFeatureRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
