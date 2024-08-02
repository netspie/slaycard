'use client'

import Image from "next/image";
import Link from "next/link";
import { useState } from "react";

export default function Home() {
  const [loading, setLoading] = useState(false)

  const fetchDataFromApi = async () => {
    setLoading(true)
    try {
      const response = await fetch("api/games/game-1", {
        headers: {
          Accept: "application/json",
          method: "GET"
        }
      })
  
      if (response) {
        const data = response.json()
        console.log(data)
      } 
    } catch (error) {
      console.log(error)
    } finally {
      setLoading(false)
    }
  }

  async function savePost(data: FormData) {

    const title = data.get('title')
    const description = data.get('description')

    const response = await fetch('http://localhost:3000/api/games',
    {
      method: 'POST',
      body: JSON.stringify({ title, description }),
      cache: 'no-cache'
    })

    if (response.ok) {
      const posts = await response.json()
      console.log(posts)
    }
  }

  return (
    <>
    <main className="flex w-full h-full items-center justify-center flex-col gap-10">
      <section className="flex flex-col gap-3 items-center">
        <h1 className="font-bold">Slaycard</h1>
        <h4 className="">Slaycard - Turn Based Combat Game</h4>
      </section>
      <Link href="/games/test" className="px-10 py-2 text-white bg-red-700 font-bold rounded-md">Play</Link>
      <div>
        <div className="flex gap-3">
          <label className="">Playing: </label>
          <span className="">234</span>
        </div>
        <div className="flex gap-3">
          <label className="">Waiting: </label>
          <span className="">473</span>
        </div>
      </div>
    </main>
    </>
  );
}
