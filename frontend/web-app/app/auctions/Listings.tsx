import React from 'react'

// server side fetching. This will be called from our node.js server
async function getData() {
    // This fetch also caches
    const res = await fetch("http://localhost:6001/search");

    if(!res.ok) throw new Error('Failed to fetch data');

    return res.json();
}

export default async function Listings() {
    const data = await getData();

  return (
    <div>
        {JSON.stringify(data, null, 2)}
    </div>
  )
}
