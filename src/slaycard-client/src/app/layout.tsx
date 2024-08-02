import type { Metadata } from "next";
import { Playfair } from "next/font/google";
import "./globals.css";

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
        <div className="fixed bg-repeat w-full h-full bg-[length:512px_512px]  bg-black"></div>
        {children}
      </body>
    </html>
  );
}
