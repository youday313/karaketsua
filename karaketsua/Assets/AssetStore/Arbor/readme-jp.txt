-----------------------------------------------------
            Arbor: State Diagram Editor
          Copyright (c) 2014 Cait Sith Ware
          http://caitsithware.com/wordpress/
          support@caitsithware.com
-----------------------------------------------------

Arborを購入いただきありがとうございます！

【更新方法】

1. 更新前に必ずプロジェクトのバックアップを取ってください。
2. 念のため、メニューのFile > New Sceneからシーンを新規作成しておきます。
3. 既にインポートされているArborフォルダを削除。
4. Arborをインポート。

【主な流れ】

1. GameObjectにArborFSMをアタッチ。
2. ArborFSMのインスペクタにあるOpen Editorボタンをクリック
3. Arbor EditorでStateを作成。
4. StateにBehaviourをアタッチ。
5. BehaviourからStateへの遷移を接続。

【JavascriptやBooを使用する場合】

1. Assets以下にPluginsフォルダがない場合、Pluginsフォルダを作成
2. Pluginsフォルダの中にArborフォルダを作成
3. Arbor/Internal/にあるScriptsフォルダを作成したArborフォルダの中に移動し、名前をInternalに変更。
4. Arbor/Core/にあるScriptsフォルダを作成したArborフォルダの中に移動し、名前をCoreに変更。

【サンプルシーン】

サンプルシーンは以下にあります。
Assets/Arbor/Examples/Scenes/

【ドキュメント】

詳しいドキュメントはこちらをご覧ください。
http://arbor.caitsithware.com/

【サポート】

フォーラム : http://forum-arbor.caitsithware.com/

メール : support@caitsithware.com

【更新履歴】

Ver 1.7.7p2:
* Arbor Editor
- Fix : Unity5.2.1以降でエラーが出るのを修正。

Ver 1.7.7p1:
* Arbor Editor
- Fix : ステートとコメントの作成と削除がUndoできなかったのを修正。

Ver 1.7.7:
* Arbor Editor
- Add : ParameterContainerでGameObjectを保持できるように対応。
- Change : 自分自身のステートへ遷移できるように変更。
- Change : 挙動の背景を変更。
- Change : ListGUIの背景を変更。
- Change : コメントノードを内容によってリサイズするように変更。
- Fix : Undo周りのバグ修正
- Fix : 常駐ステートが開始ステートに設定できたのを修正。
- Other : グリッドなどの設定をプロジェクトごとではなくUnityのメジャーバージョンごとに保存するように対応。

* 組み込み挙動
- Add : Collision/OnCollisionEnterStore
- Add : Collision/OnCollisionExitStore
- Add : Collision/OnControllerColliderHitStore
- Add : Collision/OnTriggerEnterStore
- Add : Collision/OnTriggerExitStore
- Add : Collision2D/OnCollisionEnter2DStore
- Add : Collision2D/OnCollisionExit2DStore
- Add : Collision2D/OnTriggerEnter2DStore
- Add : Collision2D/OnTriggerExit2DStore
- Add : GameObject/FindGameObject
- Add : GameObject/FindWithTagGameObject
- Add : UITweenPositionに相対指定できるように追加。
- Add : UITweenSizeに相対指定できるように追加。
- Change : BroadcastMessageGameObjectの値をFlexibleIntなどを使用するように対応。
- Change : CalcAnimatorParameterの値をFlexibleIntなどを使用するように対応。
- Change : CalcParameterの値をFlexibleIntなどを使用するように対応。
- Change : ParameterTransitionの値をFlexibleIntなどを使用するように対応。
- Change : SendMessageGameObjectの値をFlexibleIntなどを使用するように対応。
- Change : SendMessageUpwardsGameObjectの値をFlexibleIntなどを使用するように対応。
- Change : AgentEscapeをArborGameObjectに対応。
- Change : AgentFllowをArborGameObjectに対応。
- Change : ActivateGameObjectをFlexibleGameObjectに対応。
- Change : BroadastMessageGameObjectをFlexibleGameObjectに対応。
- Change : DestroyGameObjectをFlexibleGameObjectに対応。
- Change : LookatGameObjectをFlexibleGameObjectに対応。
- Change : SendMessageGameObjectをFlexibleGameObjectに対応。
- Change : SendMessageUpwardsGameObjectをFlexibleGameObjectに対応。
- Change : BroadcastTriggerをFlexibleGameObjectに対応。
- Change : SendTriggerGameObjectをFlexibleGameObjectに対応。
- Change : SendTriggerUpwardsをFlexibleGameObjectに対応。
- Change : InstantiateGameObjectで生成したオブジェクトをパラメータに格納できるように対応。

* スクリプト
- Add : FlexibleInt実装
- Add : FlexibleFloat実装
- Add : FlexibleBool実装
- Add : FlexibleGameObject実装
- Add : ContextMenuを使えるように対応。

* その他
- Change : Parameter関連をCoreフォルダとInternalフォルダに移動。
- Other : コンポーネントにアイコン設定。

Ver 1.7.6:
* Arbor Editor
- Add : StateLinkに名前設定追加。
- Add : StateLinkに即時遷移フラグ追加。
- Fix : 挙動追加での検索文字列が保存できていなかったのを修正。
- Other : 挙動追加を開いた際、検索バーにフォーカスが移るように対応。
- Other : 挙動追加での並び順で、グループが先に来るように調整。

* コンポーネント
- Add : GlobalParameterContainer

* 組み込み挙動
- Add : Audio/PlaySound
- Add : Audio/StopSound
- Add : Collision/OnCollisionEnterDestroy
- Add : Collision/OnCollisionExitDestroy
- Add : Collision/OnControllerColliderHitDestroy
- Add : Collision2D/OnCollisionEnter2DDestroy
- Add : Collision2D/OnCollisionExit2DDestroy
- Add : GameObject/BroadcastMessageGameObject
- Add : GameObject/SendMessageUpwardsGameObject
- Add : Physics/AddForceRigidbody
- Add : Physics/AddVelocityRigidbody
- Add : Physics2D/AddForceRigidbody2D
- Add : Physics2D/AddVelocityRigidbody2D
- Add : Renderer/SetSprite
- Add : Transition/Collision/OnCollisionEnterTransition
- Add : Transition/Collision/OnCollisionExitTransition
- Add : Transition/Collision/OnCollisionStayTransition
- Add : Transition/Collision/OnControllerColliderHitTransition
- Add : Transition/Collision2D/OnCollisionEnter2DTransition
- Add : Transition/Collision2D/OnCollisionExit2DTransition
- Add : Transition/Collision2D/OnCollisionStay2DTransition
- Add : Transition/Input/ButtonTransition
- Add : Transition/Input/KeyTransition
- Add : Transition/Input/MouseButtonTransition
- Add : Transition/ExistsGameObjectTransition
- Add : Trigger/BroadcastTrigger
- Add : Trigger/SendTriggerGameObject
- Add : Trigger/SendTriggerUpwards
- Add : Tween/TweenRigidbody2DPosition
- Add : Tween/TweenRigidbody2DRotation
- Add : Tween/TweenTextureOffset
- Add : UI/UISetSlider
- Add : UI/UISetSliderFromParameter
- Add : UI/UISetToggle
- Add : UI/UISetToggleFromParameter
- Add : TimeTransitionに現在時間をプログレスバーで表示するように追加。
- Add : Tween終了時に遷移できるように追加。
- Add : TweenPositionに相対指定できるように追加。
- Add : TweenRotationに相対指定できるように追加。
- Add : TweenScaleに相対指定できるように追加。
- Add : TweenRigidbodyPositionに相対指定できるように追加。
- Add : TweenRigidbodyRotationに相対指定できるように追加。
- Fix : OnTriggerExit2DDestroyがCollisionにあったのを修正。
- Fix : CalcAnimatorParameterのfloatValueがintになっていたのを修正。
- Fix : CalcParameterのfloatValueがintになっていたのを修正。
- Fix : ParameterTransitionのfloatValueがintになっていたのを修正。
- Other : SetRigidbodyVelocityをSetVelocityRigidbodyに改名。
- Other : SetRigidbody2DVelocityをSetVelocityRigidbody2Dに改名。

* スクリプト
- Add : FixedImmediateTransition属性で即時遷移フラグを変更できないように対応。

* その他
- Add : Example9としてGlobalParameterContainerのサンプル追加。
- Fix : TagsにCoinが追加されていたので修正。

Ver 1.7.5:
* Arbor Editor
- Fix : グリッドが正しく表示されない時があるのを修正。
- Other : ステートリストの横幅をリサイズできるように対応。

* 組み込み挙動
- Add : Collision/OnTriggerEnterDestroy
- Add : Collision/OnTriggerExitDestroy
- Add : Collision2D/OnTriggerEnter2DDestroy
- Add : Collision2D/OnTriggerExit2DDestroy
- Add : GameObject/LookAtGameObject
- Add : Parameter/SetBoolParameterFromUIToggle
- Add : Parameter/SetFloatParameterFromUISlider
- Add : Physics/SetRigidbodyVelocity
- Add : Physics2D/SetRigidbody2DVelocity
- Add : Transition/EventSystems/OnPointerClickTransition
- Add : Transition/EventSystems/OnPointerDownTransition
- Add : Transition/EventSystems/OnPointerEnterTransition
- Add : Transition/EventSystems/OnPointerExitTransition
- Add : Transition/EventSystems/OnPointerUpTransition
- Add : Tween/TweenCanvasGroupAlpha
- Add : Tween/TweenRigidbodyPosition
- Add : Tween/TweenRigidbodyRotation
- Add : UI/UISetImage
- Add : UI/UISetTextFromParameter
- Add : InstantiateGameObjectで生成時の初期Transformを指定できるように追加。
- Fix : CalcParameterでBool型の場合に正しく動作しなかったのを修正。
- Fix : SendEventGameObjectで呼び出す方をわざわざ指定しないように修正。

* スクリプト
- Add : Parameterにvalueプロパティ追加。
- Add : IntParameterReference追加。
- Add : FloatParameterReference追加。
- Add : BoolParameterReference追加。

* その他
- Add : HierarchyのCreateボタンからArborFSM付きGameObjectを作れるように追加。
- Add : HierarchyのCreateボタンからParameterContainer付きGameObjectを作れるように追加。
- Add : HierarchyのCreateボタンからAgentController付きGameObjectを作れるように追加。
- Add : Example7としてコインプッシャーゲーム追加。
- Add : Example8としてEventSystemのサンプル追加。
- Other : フォルダ整理。

Ver 1.7.4:
- Add : Agent系Behaviour追加。
- Add : uGUI系Behaviour追加。
- Add : uGUI系Tween追加。
- Add : SendEventGameObject追加。
- Add : SendMessageGameObjectに値渡し機能追加。
- Fix : AnimatorParameterReferenceの参照先がAnimatorControllerを参照していなかったときにエラーが出るのを修正。
- Other : uGUI対応に伴いUnity最低動作バージョンを4.6.7f1に引き上げ。

Ver 1.7.3:
- Add : OnMouse系Transition追加
- Fix : 選択ステートへの移動時のスクロール位置修正
- Other : ステートリストを名前順でソートするように変更。
- Other : Arbor Editorの左上方向へも無限にステートを配置できるように変更。
- Other : マニュアルサイトを一新。

Ver 1.7.2:
- Add : ArborEditorにコメントノードを追加。
- Add : 挙動追加時に検索できるように対応。
- Add : CalcAnimatorParameter追加。
- Add : AnimatorStateTransition追加。
- Add : 遷移線を右クリックで遷移元と遷移先へ移動できるように追加。
- Fix : Prefab元に挙動追加するとPrefab先に正しく追加されないのを修正。
- Other : ForceTransitionをGoToTransitionに改名。
- Other : 挙動追加で表示される組み込みBehaviourの名前を省略しないように変更。
- Other : 組み込みBehaviourをAdd Componentに表示しないように変更。

Ver 1.7.1:
- Add : ステートリストを追加。
- Add : ParamneterReferenceのPropertyDrawerを追加。
- Add : 要素の削除ができるリスト用のGUI、ListGUIを追加。
- Fix : CalcParameterのboolValueがintになっていたのを修正。

Ver 1.7.0;
- Add : パラメータコンテナ。
- Fix : OnStateBegin()で状態遷移した場合、それより下のBehaviourを実行しないように修正。

Ver 1.6.3f1:
- Unity5 RC3でエラーが出るのを修正。
- Unity5 RC3対応により、OnStateEnter/OnStateExitをOnStateBegin/OnStateEndに改名。

Ver 1.6.3:
- Transitionにforceフラグを追加。trueにすると呼び出し時にその場で遷移するようにできる。
- ソースコードへドキュメントコメント埋め込み。
  Player Settings の Scripting Define Symbols に ARBOR_DOC_JA を追加すると日本語でドキュメントコメントが見れるようになります。
- スクリプトリファレンスをAssets/Arbor/Docsに配置。
  解凍してindex.htmlを開いてください。

Ver 1.6.2:
- FIX : OnStateEnterでステート遷移できないのを修正。

Ver 1.6.1:
- FIX : Mac環境でGridボタン押すとエラーが表示される。

Ver 1.6:
- ADD : 常駐ステート。
- ADD : 多言語対応。
- ADD : ArborFSMに名前を付けられるように対応。
- FIX : グリッドサイズを変更してもスナップ間隔に反映されない。
- FIX : ArborFSMのコンポーネントをコピー＆ペーストした際にStateBehaviourが消失する問題の対処。
- FIX : SendTriggerを現在有効なステートにのみ送るように変更。
- FIX : ArborFSMを無効にしてもStateBehaviourが動き続ける。

Ver 1.5:
- ADD : ステートの複数選択に対応。
- ADD : ショートカットキーに対応。
- ADD : グリッド表示対応。
- FIX : Behaviour追加時にデフォルトで広げた状態にする。
- FIX : StateLinkのドラッグ中にステートへのマウスオーバーがずれて反応する。
 
ver 1.4:
- ADD : Tween系Behaviour追加。
 - Tween / Color
 - Tween / Position
 - Tween / Rotation
 - Tween / Scale
- ADD : Add Behaviourに表示されないようにするHideBehaviour属性追加。
- ADD : Behaviourのヘルプボタンから組み込みBehaviourのオンラインヘルプ表示。

ver 1.3:
- ADD : 組み込みBehaviour追加。
 - Audio / PlaySoundAtPoint
 - GameObject / SendMessage
 - Scene / LoadLevel
 - Transition / Force
- ADD : シーンを跨いだコピー&ペースト。
- FIX : Stateをコピーしたあとシーンを保存するとメモリリークの警告が表示される。
- FIX : StateLinkの接続ドラッグ中に画面スクロールすると矢印が残る。

ver 1.2:
- ADD : StateBehaviourの有効チェックボックス。
- FIX : Arbor Editorの最大化を解除するとエラーが出る。
- FIX : 生成したC#スクリプトを編集すると改行コードの警告が出る。

ver 1.1:
- ADD : JavaScriptとBooのスクリプト生成。
- ADD : Stateのコピー＆ペースト。
- ADD : StateBehaviourのコピー＆ペースト。
- FIX : スクリプトがMissingになったときの対応。
- FIX : StateLinkの配列が表示されないのを修正。

ver 1.0.1:
- FIX : Unity4.5でのエラー。
- FIX : エディタ上での実行時にArbor Editorが再描画されない。
- FIX : ArborFSMのInspector拡張のクラス名。
