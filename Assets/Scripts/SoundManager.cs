using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] SoundPackSO soundsPack;
    [SerializeField] AudioClip transitionMusic;
    [SerializeField] AudioClip finalMusic;
    public static SoundManager instance { get; private set; }
    private AudioSource audioSource;
    private float deltaVolume;
    private float originalVolume;
    private const string DELTA_VOLUME_KEY = "DeltaVolume";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        audioSource = GetComponent<AudioSource>();
        deltaVolume = PlayerPrefs.GetFloat(DELTA_VOLUME_KEY, 1f);
    }

    private void Start()
    {
        audioSource.volume *= deltaVolume;
        originalVolume = audioSource.volume;
    }


    private void OnEnable()
    {
        audioSource.volume *= deltaVolume;
        originalVolume = audioSource.volume;

        FlashLight.OnFlashLightOn += PlayFlashlightSound;
        FlashLight.OnFlashLightOff += PlayFlashlightSound;
        Bookshelf.OnBookCollected += PlayBookCollectedSound;
        Bookshelf.OnBookCollected += StartBookTransitionMusic;
        StressSlider.OnStressFulled += PlayPlayerScreamSound;
        Enemy.OnPlayerHeard += PlayMonsterScreamSound;
        BatterySlider.OnBatteryDied += PlayBatteryDiedSound;
        Enemy.OnPlayerDied += PlayScreamerSound;
        MainMenu.OnSliderSaved += ChangeVolume;
        Door.OnDoorOpened += PlayDoorOpenSound;
        Inventory.OnKeyCollected += PlayKeySound;
        PauseMenuController.OnButtonClicked += PlayButtonClickSound;
        MainMenu.OnButtonPressed += PlayButtonClickSound;
    }


    private void OnDisable()
    {
        FlashLight.OnFlashLightOn -= PlayFlashlightSound;
        FlashLight.OnFlashLightOff -= PlayFlashlightSound;
        Bookshelf.OnBookCollected -= PlayBookCollectedSound;
        Bookshelf.OnBookCollected -= StartBookTransitionMusic;
        StressSlider.OnStressFulled -= PlayPlayerScreamSound;
        Enemy.OnPlayerHeard -= PlayMonsterScreamSound;
        BatterySlider.OnBatteryDied -= PlayBatteryDiedSound;
        Enemy.OnPlayerDied -= PlayScreamerSound;
        MainMenu.OnSliderSaved -= ChangeVolume;
        Door.OnDoorOpened -= PlayDoorOpenSound;
        Inventory.OnKeyCollected -= PlayKeySound;
        PauseMenuController.OnButtonClicked -= PlayButtonClickSound;
        MainMenu.OnButtonPressed -= PlayButtonClickSound;
    }


    public void PlaySound(AudioClip sound, Vector3 position, float volume)
    {
        AudioSource.PlayClipAtPoint(sound, position, deltaVolume * volume);
    }


    public void PlaySound(AudioClip[] soundsArray, Vector3 position, float volume)
    {
        AudioSource.PlayClipAtPoint(soundsArray[Random.Range(0, soundsArray.Length)], position, deltaVolume * volume);
    }


    public void PlayFootStepsSound(Vector3 position, float volume)
    {
        PlaySound(soundsPack.walking, position, volume);
    }


    public void PlayFlashlightSound()
    {
        Player player = Player.instance;
        float volume = 1;
        PlaySound(soundsPack.flashLight, player.transform.position, volume);
    }


    public void PlayBookCollectedSound(Loot item)
    {
        Player player = Player.instance;
        float volume = 1;
        PlaySound(soundsPack.bookCollected, player.transform.position, volume);
    }


    public void PlayPlayerScreamSound()
    {
        Player player = Player.instance;
        float volume = 1;
        PlaySound(soundsPack.screamingPlayer, player.transform.position, volume);
    }


    public void PlayMonsterScreamSound(Vector3 position)
    {
        float volume = 2f;
        PlaySound(soundsPack.screamingMonster, position, volume);
    }


    public void PlayMonsterWalkingSound(Vector3 position)
    {
        float volume = 0.3f;
        PlaySound(soundsPack.monsterWalking, position, volume);
    }


    public AudioClip GetMosterWalkingClip()
    {
        return soundsPack.monsterWalking[Random.Range(0, soundsPack.monsterWalking.Length)];
    }


    public void PlayBatteryDiedSound()
    {
        Player player = Player.instance;
        float volume = 1f;
        PlaySound(soundsPack.batteryDied, player.transform.position, volume);
    }


    public void PlayScreamerSound(GameObject enemy)
    {
        float volume = 0.5f;
        PlaySound(soundsPack.screamer, Camera.main.transform.position, volume);
    }


    public void PlayButtonClickSound()
    {
        float volume = 1f;
        PlaySound(soundsPack.buttonClick, Camera.main.transform.position, volume);
    }


    public void PlayDoorOpenSound(Vector3 position)
    {
        float volume = 1f;
        PlaySound(soundsPack.doorOpen, position, volume);
    }

    public void PlayTypingSound()
    {
        float volume = 0.01f;
        PlaySound(soundsPack.typing, Camera.main.transform.position, volume);
    }

    public void PlayScreamSchoolSceneSound()
    {
        float volume = 0.5f;
        PlaySound(soundsPack.screamSchoolScene, Camera.main.transform.position, volume);
    }


    public void PlayBellSchoolSceneSound()
    {
        float volume = 0.5f;
        PlaySound(soundsPack.ringSchoolScene, Camera.main.transform.position, volume);
    }


    public void PlayLaughSchoolSceneSound()
    {
        float volume = 0.5f;
        PlaySound(soundsPack.laughSchoolScene, Camera.main.transform.position, volume);
    }

    public void PlayKeySound(Vector3 position)
    {
        float volume = 1f;
        PlaySound(soundsPack.key, position, volume);
    }

    public void ChangeVolume(float savedVolumeValue)
    {
        deltaVolume = savedVolumeValue / 4;
        audioSource.volume = originalVolume * deltaVolume;
        PlayerPrefs.SetFloat(DELTA_VOLUME_KEY, deltaVolume);
        PlayerPrefs.Save();
    }


    public void StartMusicTransition(AudioClip newClip, float transitionTime = 1.5f)
    {
        StartCoroutine(MusicTransition(newClip, transitionTime));
    }


    IEnumerator MusicTransition(AudioClip newClip, float transitionTime)
    {
        float elapsedTime = 0;
        float initialVolume = audioSource.volume;

        while (elapsedTime < transitionTime)
        {
            elapsedTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(initialVolume, 0f, elapsedTime / transitionTime);
            yield return null;
        }

        audioSource.clip = newClip;
        audioSource.Play();
        elapsedTime = 0;

        while (elapsedTime < transitionTime)
        {
            elapsedTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0f, initialVolume, elapsedTime / transitionTime);
            yield return null;
        }
    }


    private void StartBookTransitionMusic(Loot loot)
    {
        StartCoroutine(BookTransitionMusic(loot));
    }


    IEnumerator BookTransitionMusic(Loot loot)
    {
        StartMusicTransition(transitionMusic);
        yield return new WaitForSeconds(transitionMusic.length);
        StartMusicTransition(finalMusic);
    }

}
