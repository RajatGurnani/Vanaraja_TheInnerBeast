using UnityEngine;

public class GameSocials : MonoBehaviour
{
    private string facebookURL = "https://www.facebook.com/profile.php?id=100088596794084";
    private string discordURL = "https://discord.gg/HTPGwMccwM";
    private string instagramURL = "https://www.instagram.com/nerdyqueststudio";
    private string twitterURL = "https://twitter.com/nerdy_quest?s=20";
    private string youtubeURL = "https://www.youtube.com/@nerdyquestofficial";
    private string linkedinURL = "";

    public void OpenDiscord() => Application.OpenURL(discordURL);
    public void OpenTwitter() => Application.OpenURL(twitterURL);
    public void OpenFacebook() => Application.OpenURL(facebookURL);
    public void OpenInstagram() => Application.OpenURL(instagramURL);
    public void OpenYoutube() => Application.OpenURL(youtubeURL);
    public void OpenLinkedin() => Application.OpenURL(linkedinURL);
}