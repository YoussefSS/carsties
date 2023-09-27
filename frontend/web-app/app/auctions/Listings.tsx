import React from 'react'
import AuctionCard from './AuctionCard';

// server side fetching. This will be called from our node.js server
async function getData() {
    // This fetch also caches
    const res = await fetch("http://localhost:6001/search?pageSize=10");

    if(!res.ok) throw new Error('Failed to fetch data');

    return res.json();
}

export default async function Listings() {
    const data = await getData();

  return (
    <div className='grid grid-cols-4 gap-6'>
        {data && data.results.map((auction: any) => (
            <AuctionCard auction={auction} key={auction.id}/>
        ))}
    </div>
  )
}
