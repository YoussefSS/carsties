import NextAuth, { NextAuthOptions } from 'next-auth';
import DuendeIdentityServer6 from 'next-auth/providers/duende-identity-server6';

export const authOptions: NextAuthOptions = {
    session: {
        strategy: 'jwt',
    },
    providers: [
        DuendeIdentityServer6({
            id: 'id-server', // how we identify our provider inside our app
            clientId: 'nextApp', // matches what we called it in IdentityServer (Config.cs)
            clientSecret: 'secret',
            issuer: 'http://localhost:5000',
            authorization: { params: { scope: 'openid profile auctionApp' } },
            idToken: true,
        }),
    ],
};

const handler = NextAuth(authOptions);
export { handler as GET, handler as POST };
