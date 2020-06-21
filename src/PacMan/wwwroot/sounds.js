class SoundPlayer {
    constructor() {
        this.sounds = [];
    }

    loadSound(name, path) {
        const soundEffect = new SoundEffect(name);
        soundEffect.loadSound(path);
        this.sounds[name] = soundEffect;
    }

    getSound(name) {
        return this.sounds[name];
    }

    stop(name) {
        this.getSound(name).stop();
    }

    getVolume(name) {
        return this.getSound(name).getVolume();
    }

    setVolume(name, value) {
        this.getSound(name).setVolume(value);
    }

    isPlaying(name) {
        return this.getSound(name).isPlaying();
    }

    setLoop(name, value) {
        this.getSound(name).setLoop(value);
    }

    mute(name) {
        return this.getSound(name).mute();
    }

    unmute(name) {
        return this.getSound(name).unmute();
    }

    isLoaded(name) {
        return this.getSound(name).isLoaded();
    }

    play(name) {
        this.getSound(name).play();
    }

    tryPlay(name) {
        this.getSound(name).tryPlay();
    }
};

class SoundEffect {
    
    constructor(name) {
        this._name = name;
    }

    loadSound(path) {
        this._howl= new Howl({
            src: path
        });

        this._howl.on("load",
            () => {

                this._loaded = true;
            });

        this._howl.on("loaderror",
            () => {

                this._loaded = true;
            });

        this._howl.on("end",
            () => {
                window.theInstance.invokeMethodAsync('SoundEnded', this._name);
            });
    }

    getVolume() {
        return this._howl.volume();
    }

    setVolume(value) {
        this._howl.volume(value);
    }

    isPlaying() {
        return this._howl.playing();
    }

    setLoop(value) {
        this._howl.loop(value);
    }

    mute() {
        return this._howl.mute(true);
    }

    unmute() {
        return this._howl.mute(false);
    }

    isLoaded() {
        return this._loaded;
    }

    stop() {
        this._howl.stop();
    }

    play() {
        if (!this.isLoaded) {
            throw new Error(`Not loaded!`);
        }

        if (this._howl.loop() && this._howl.playing()) {
            return;
        }

        this._howl.play();
    }

    tryPlay() {
        if (this._howl.loop() && !this._howl.playing()) {
            return;
        }

        this._howl.play();
    }
}

window.soundPlayer = new SoundPlayer();
