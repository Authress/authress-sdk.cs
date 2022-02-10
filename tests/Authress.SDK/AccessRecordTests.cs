using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Authress.SDK.Client;
using Authress.SDK.DTO;
using FluentAssertions;
using Xunit;

namespace Authress.SDK.UnitTests
{
    public class AccessRequestTests
    {
        [Fact]
        public void ValidateStatusEnumStringValue()
        {
            AccessRecord.StatusEnum.ACTIVE.ToString().Should().Be("ACTIVE");
        }
    }
}
