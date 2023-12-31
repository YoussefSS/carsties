'use server';

import { Auction, PagedResult } from '@/types';
import { getTokenWorkaround } from './authActions';
import { fetchWrapper } from '../lib/fetchWrapper';
import { FieldValues } from 'react-hook-form';
import { revalidatePath } from 'next/cache';

// server side fetching. This will be called from our node.js server
export async function getData(query: string): Promise<PagedResult<Auction>> {
    return await fetchWrapper.get(`search/${query}`);
}

export async function updateAuctionTest() {
    const data = {
        mileage: Math.floor(Math.random() * 100000) + 1,
    };

    return await fetchWrapper.put(
        'auctions/afbee524-5972-4075-8800-7d1f9d7b0a0c',
        data
    );
}

export async function createAuction(data: FieldValues) {
    return await fetchWrapper.post('auctions', data);
}

export async function getDetailedViewData(id: string): Promise<Auction> {
    return await fetchWrapper.get(`auctions/${id}`);
}

export async function updateAuction(data: FieldValues, id: string) {
    const res = await fetchWrapper.put(`auctions/${id}`, data);
    revalidatePath(`/auctions/${id}`); // disable caching for this request
    return res;
}

export async function deleteAuction(id: string) {
    return await fetchWrapper.del(`auctions/${id}`);
}
