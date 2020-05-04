using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;
using Authress.SDK;

namespace Authress.SDK.UnitTests
{
    public class ServiceClientTokenProviderTests
    {
        private const string userId = "unit-test-user-id";
        private const string testAccessKey = "eyJrZXlJZCI6IjA3NmMyZDNmLTA0NDYtNGNmOC04YWQ5LTY4MDVlMTU5NTg5OSIsInByaXZhdGVLZXkiOiItLS0tLUJFR0lOIFBSSVZBVEUgS0VZLS0tLS1cbk1JSUV2UUlCQURBTkJna3Foa2lHOXcwQkFRRUZBQVNDQktjd2dnU2pBZ0VBQW9JQkFRRDltUVo0TDdaTW05bThcbkllK0FubFdzc29zb05JbDd1b090TGpKMEJDR1c2L2FhTjZxdUhtR0YrVU9yUmZJOFY0b3ZKTXBPMHF1WUEyZTVcbmVXT2d1MXdRdjBwcTQvdEYvQkp3MU1uQTlYY2FDZ3N4REw0ZFVJTTEycmY3WkNlWXRYRFl3UWhrWGNoVmdpUVhcbmdEeVVCRXFWS2g0bzhCQnRiYS9vdzB6bDg5a2Rtam9NTXRBZzJKdUxFUTBUVTVSNDFBUUJxamQrMldFSmdsbVlcbko2bEovakczdzB2R09vanlpd2VKaTZWRHdSeDVBR0FOdUh3dzZYQ0l1OHVJb0lBWlRNcFJySXozeHZyeUpjaGZcbnoxNGI5bjR6a29qbEIwbURVQ0dIblVzZjlZdHg2R0I0clR1MXdxU3Zqa1JEQjJRRUlPR2djenU5K0pMcDVXNitcbkVzWEt2UmpyQWdNQkFBRUNnZ0VCQUozUHIyMzFTdjJMY3NpWFdhSnhaYUZOampsYjBENTF4K0ZxUVMxZk1NUHBcblhSR0ZHS3EzN3pwZTdwUlR0N1dEU0ZPa0VsMVF6a1dQd09senQrTGJGU1M2MVlXRkQrWHlRa3VDcjNacmlrMWlcbitLbnlZeEI4L05uem5OQk0rRE1ZbmZ2VXkvTWhSVHlvK3VyQSszR2s1Z0RETC9lTHhMMUVKOWF5U2xWREZOWk1cbjJUbURqcHhtbmNOTXphZ0hVcVVpMy9xNWVaREFJU1FBNVgwL0QrWFcxVkw4LzRLUlBaaFU5bGRCWTZNYTJqUGFcblRaTE4vOVVmWDMrZnlUWnphVld1dUdwc25LUkZuRzdxZFdWM0FQaXdJT0paZU1uZ29IL2k1NTFDYzQvbFlMbVRcbllZNFZMWTVEbUVVNkNiM3ZWRXZEUWVacUpYVERHUkxvWDFxRGlBeWZwN2tDZ1lFQS8rQ0FKZXp5aWlCckZ0OW9cbmUrNFJncnhpZzJ6eE1VZFp0a0RlelAzZG13ZVNPcmkrcU5yeFdMTVZOQTNsT3ZPbDZiNG1ReTFTQ0lBc1F5Q3dcbjJGb2Q3SllRclM4QmtNR2dSeGtLbnBhcmhiRjJGUjdjN2trU3ZVci9rUnZleXlxU2p6OU5hQzYrcnFnNlNWRTFcbkl6elpac2o0aURvN1RyZ0ZwbGZsZDVBZHVyY0NnWUVBL2JnK2ZrbkpIZTFzSCtQbmFjcE9Yd001R2Rva2xmTCtcbm1uT2ZVN2hsbDQvWDkwbXRRMFJsakJZU0NsQUpxSGhRQnVLSXJ3ckQwQ29wREJlaW42eUFneGkyQmpVUUFmckxcbjJEQ2pvSlVwS2pIcGtBNStnVkJhQVgyRDBOWmhGTFpxSVJYSFJvdnl4dmEvaTVhdFQ0cXA4RTBreXhYMkkxOG5cbmoySjhtM1NiTDIwQ2dZQkp0Nk1UeWhVQ0tGN1I0eUZWK3Z0K2Y4bWQ5WWZ6VzR3RUR1SmhpbzRLdVA2dS9rU25cbm54UkRLcXprSjFDd1VEdXZnTUhEUHM1UWRxVEozaVEwNEptWWJJOTNaWUI4OU51NVFBU29OZDVLa1JyazhOUlRcbnJpZkE4MWQzdGVEVkJYbmQzUzN1NHZDNm51clQ3cHB4Z1hsY3ZHK2x4NmtJZjhuWTU1L0xkM0NwTndLQmdBbXFcbnVHN1ZYdDFPQzMyWGtGeWVnYWZyRm9UZW8rQTJ0dTZwa3h0OGZocHRONXhMYVZlVHhvNjAxSkVpQll3dXNWWGhcbjBiVmhvcDVPek91U0J2Y3dlbUVFVXdNZmlIR2EzYU5xRHdIeVRQUTNuSitKZmRadGVsQnVPTlIvSm9uRWZYeXZcbk9MMStYWXlwSUJrd2I0QUZWMzQ1WWpwK3ByY29TL2lSbHcvUlBJeHRBb0dBYVZMRG9xcTh3OGozOXRjYVYvWDFcbmJxY3pLTWRuRTNRZjBUZURyWW15UXh0MjBiWVZpQnUwQUQrcWhsOUI3aFgrcFM0Y2RCSkV4Z0ZUZ256YjhvYk9cbk5rUnpLd2JNanVWa2k3alNrdEliNldpTjZpemR3aWZCbnVjVlVvZkY1dXUzMUpCL2JUZWJuaXJmemttQWFoVjBcbjRvd2EyaWhWblcwWnl3eHBsaU52RnIwPVxuLS0tLS1FTkQgUFJJVkFURSBLRVktLS0tLVxuIiwiYXVkaWVuY2UiOiJ1bmRlZmluZWQuYWNjb3VudHMuYXV0aHJlc3MuaW8iLCJjbGllbnRJZCI6ImMyNWE5MWYwLTJmZTEtNDQ1MS05ZTQ5LTRmZTI4ODEzOWZjOSJ9";

        public class TestCase
        {

        }

        public static TestData<TestCase> TestCases
        {
            get
            {
                return new TestData<TestCase>
                {
                    {
                        "successful conversion",
                        new TestCase()
                    }
                };
            }
        }

        [Theory, MemberData(nameof(TestCases))]
        public async Task GetBearerToken(string testName, TestCase testCase)
        {
            var tokenProvider = new AuthressClientTokenProvider(testAccessKey);
            var token = await tokenProvider.GetBearerToken();

            token.Should().NotBeNull(testName, testCase);
        }
    }
}
