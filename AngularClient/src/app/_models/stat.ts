export interface Stat {
    type: string; //daily, weekly, monthly, yearly
    mostListenedGenre: string;
    minsSpent: number;
    mostListenedArtist: string;
    mostListenedSong: string;
    numOfListenedGenre: number;
    secondMostListenedGenre: string;
    secondMostListenedArtist: string;
    secondMostListenedSong: string;

    thirdMostListenedGenre: string;
    thirdMostListenedArtist: string;
    thirdMostListenedSong: string;

    fourthMostListenedGenre: string;
    fourthMostListenedArtist: string;
    fourthMostListenedSong: string;

    fifthMostListenedGenre: string;
    fifthMostListenedArtist: string;
    fifthMostListenedSong: string;
    //still expanding
}