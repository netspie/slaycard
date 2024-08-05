import type { Metadata } from "next";
import { Playfair } from "next/font/google";
import "./globals.css";
import Image from "next/image";
import Link from "next/link";

const inter = Playfair({ subsets: ["latin"] });

export const metadata: Metadata = {
  title: "Slaycard",
  description: "Slaycard - Turn Based Combat Game",
};

export default function RootLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  const htmlClass = `min-h-full h-full`;
  const bodyClass = `${inter.className} min-h-full h-full`;

  return (
    <html lang="en" className={htmlClass}>
      <body className={bodyClass}>
        <div className="fixed flex p-8">
          <Link
            href="/"
            className="absolute left-10 text-white text-2xl p-0 m-0 z-[1] uppercase font-bold"
          >
            Slaycard
          </Link>
        </div>
        <div className="fixed w-full h-full -z-50">
          <Image
            className="absolute w-full h-full -z-50 pointer-events-none"
            src="/bgs/battle-bg.jpg"
            alt="Dope"
            layout="fill"
            objectFit="cover"
          />
          <div className="bg-black w-full h-full opacity-[95%]" />
        </div>
        <div className="fixed w-full h-full -z-10"></div>
        {children}
      </body>
    </html>
  );
}
