using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using Zenject;

namespace DuelGame
{
    public class BattleSFXController : IInitializable, IDisposable
    {
        private readonly BattleManager _battleManager;
        private readonly IAudioPlayer _audioPlayer;
        
        private readonly float _finishBattleSoundVolume = 0.9f;
        
        private AudioClip _deathSE;
        private AudioClip _attackSE;
        private AudioClip _hitSE;
        private AudioClip _youDiedSE;
        private AudioClip _enemyFelledSE;
        
        private BaseHero _player1;
        private BaseHero _player2;
        
        public BattleSFXController(BattleManager battleManager, SoundEffects soundEffects, IAudioPlayer audioPlayer)
        {
            _battleManager = battleManager;
            _audioPlayer = audioPlayer;
            
            _deathSE = soundEffects.DeathSE;
            _attackSE = soundEffects.AttackSE;
            _hitSE = soundEffects.HitSE;
            _youDiedSE = soundEffects.YouDiedSE;
            _enemyFelledSE = soundEffects.EnemyFelledSE;
        }

        public void Initialize()
        {
            _battleManager.OnPlayersSpawned += OnPlayerSpawnedHandler;
            _battleManager.OnPlayerDead += PlayFinishBattleAudio; 
        }

        public void Dispose()
        {
            _battleManager.OnPlayersSpawned -= OnPlayerSpawnedHandler;
            _battleManager.OnPlayerDead -= PlayFinishBattleAudio; 

            if (_player1 != null)
            {
                _player1.OnAttack -= PlayAttackSound;
                _player1.OnTakeDamage -= PlayTakeDamageSound;
                _player1.OnDeath -= PlayDeathSound;
            }

            if (_player2 != null)
            {
                _player1.OnAttack -= PlayAttackSound;
                _player1.OnTakeDamage -= PlayTakeDamageSound;
                _player1.OnDeath -= PlayDeathSound;
            }
        }

        private void OnPlayerSpawnedHandler(BattleState battlestate)
        {
            _player1 = _battleManager.Player1();
            _player2 = _battleManager.Player2();
            
            _player1.OnAttack += PlayAttackSound;
            _player1.OnTakeDamage += PlayTakeDamageSound;
            _player1.OnDeath += PlayDeathSound;
            
            _player2.OnAttack += PlayAttackSound;
            _player2.OnTakeDamage += PlayTakeDamageSound;
            _player2.OnDeath += PlayDeathSound;
        }

        private void PlayFinishBattleAudio(Players? playerwholost)
        {
            Debug.Log("Playing finish battle audio");
            if (playerwholost == Players.Player1)
            {
                _audioPlayer.PlayOneShot(_youDiedSE, _finishBattleSoundVolume);
            }
            else if(playerwholost == Players.Player2)
            {
                _audioPlayer.PlayOneShot(_enemyFelledSE, _finishBattleSoundVolume);
            }
        }
        
        private void PlayDeathSound(BaseHero _, Players player)
        {
            _audioPlayer.PlayOneShot(_deathSE);
        }

        private void PlayTakeDamageSound(BaseHero _, float __, float ___, float ____, bool _____)
        {
            _audioPlayer.PlayOneShot(_hitSE);
        }

        private void PlayAttackSound(BaseHero _)
        {
            _audioPlayer.PlayOneShot(_attackSE);
        }
    }
}