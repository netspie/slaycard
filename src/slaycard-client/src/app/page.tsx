"use client";

import Link from "next/link";

export default function Home() {
  return (
    <>
      <main className="flex w-full h-full items-center justify-center flex-col gap-10">
        <section className="flex flex-col gap-3 items-center">
          <h1 className="text-gray-100 font-bold">Slaycard</h1>
          <h4 className="text-gray-400">Turn Based Combat Game</h4>
        </section>
        <Link
          href="/combats/xyz"
          className="px-10 py-2 text-white bg-red-700 font-bold rounded-md"
        >
          Play Demo
        </Link>
        {/* <div className="">
          <div className="flex gap-3">
            <label className="">Playing: </label>
            <span className="">234</span>
          </div>
          <div className="flex gap-3">
            <label className="">Waiting: </label>
            <span className="">473</span>
          </div>
        </div> */}
      </main>
    </>
  );
}
