'use server';

import { Auction, PagedResult } from '@/types';

// server side fetching. This will be called from our node.js server
export async function getData(
    pageNumber: number = 1
): Promise<PagedResult<Auction>> {
    // This fetch also caches
    const res = await fetch(
        `http://localhost:6001/search?pageSize=4&pageNumber=${pageNumber}`
    );

    if (!res.ok) throw new Error('Failed to fetch data');

    return res.json();
}
