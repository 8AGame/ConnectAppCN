using System;
using System.Collections.Generic;
using ConnectApp.constants;
using ConnectApp.models;
using ConnectApp.utils;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;

namespace ConnectApp.components {
    public class EventDetail : StatelessWidget {
        public EventDetail(
            bool isShowImage,
            IEvent eventObj = null,
            Action<string> openUrl = null,
            Action<string> playVideo = null,
            Widget topWidget = null,
            Key key = null
        ) : base(key) {
            this.eventObj = eventObj;
            this.openUrl = openUrl;
            this.playVideo = playVideo;
            this.isShowImage = isShowImage;
            this.topWidget = topWidget;
        }

        readonly IEvent eventObj;
        readonly bool isShowImage;
        readonly Action<string> openUrl;
        readonly Action<string> playVideo;
        readonly Widget topWidget;


        public override Widget build(BuildContext context) {
            return new Container(child: this._buildContent(context));
        }

        Widget _buildContent(BuildContext context) {

            var items = new List<Widget> {
                this._buildHeadImage(), this._buildContentHead()
            };
            if (this.topWidget!=null) {
                items.Insert(0,this.topWidget);
            }
            items.AddRange(ContentDescription.map(context, this.eventObj.content, this.eventObj.contentMap,
                this.openUrl, this.playVideo
            ));
            items.Add(this._buildContentLecturerList());
            return new Container(
                child: ListView.builder(
                    physics: new AlwaysScrollableScrollPhysics(),
                    itemCount: items.Count,
                    itemBuilder: (cxt, index) => items[index]
                )
            );
        }

        Widget _buildHeadImage() {
            if (!this.isShowImage) {
                return new Container();
            }

            var imageUrl = this.eventObj.avatar ?? "";
            return new Container(
                child: new PlaceholderImage(
                    imageUrl.EndsWith(".gif") ? imageUrl : $"{imageUrl}.1400x0x1.jpg",
                    fit: BoxFit.cover
                )
            );
        }

        Widget _buildContentHead() {
            var user = this.eventObj.user ?? new User();
            return new Container(
                padding: EdgeInsets.symmetric(horizontal: 16),
                margin: EdgeInsets.only(top: 16, bottom: 20),
                child: new Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new Text(this.eventObj.title ?? "",
                            style: CTextStyle.H4
                        ),
                        new Container(
                            margin: EdgeInsets.only(top: 20),
                            child: new Row(
                                children: new List<Widget> {
                                    new Container(
                                        margin: EdgeInsets.only(right: 8),
                                        child: Avatar.User(user.id, user, 32)
                                    ),
                                    new Column(
                                        mainAxisAlignment: MainAxisAlignment.center,
                                        crossAxisAlignment: CrossAxisAlignment.start,
                                        children: new List<Widget> {
                                            new Text(
                                                user.fullName ?? "",
                                                style: CTextStyle.PMediumBody
                                            ),
                                            new Text(
                                                $"{DateConvert.DateStringFromNow(this.eventObj.createdTime)}发布",
                                                style: CTextStyle.PSmallBody3
                                            )
                                        }
                                    )
                                }
                            )
                        )
                    }
                )
            );
        }

        Widget _buildContentLecturerList() {
            var hosts = this.eventObj.hosts;
            if (hosts == null || hosts.Count == 0) {
                return new Container();
            }

            var hostItems = new List<Widget>();
            hostItems.Add(new Container(
                margin: EdgeInsets.symmetric(horizontal: 16, vertical: 40),
                height: 1,
                color: CColors.Separator2
            ));
            hostItems.Add(new Padding(
                padding: EdgeInsets.fromLTRB(16, 0, 16, 24),
                child: new Text(
                    "讲师",
                    style: CTextStyle.H4
                )
            ));
            hosts.ForEach(host => { hostItems.Add(_buildLecture(host)); });
            return new Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: hostItems
            );
        }

        static Widget _buildLecture(User host) {
            return new Container(
                padding: EdgeInsets.symmetric(horizontal: 16),
                margin: EdgeInsets.only(bottom: 24),
                color: CColors.White,
                child: new Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new Row(
                            children: new List<Widget> {
                                new Container(
                                    margin: EdgeInsets.only(right: 8),
                                    child: Avatar.User(host.id, host, 48)
                                ),
                                new Column(
                                    mainAxisAlignment: MainAxisAlignment.center,
                                    crossAxisAlignment: CrossAxisAlignment.start,
                                    children: new List<Widget> {
                                        new Container(
                                            child: new Text(
                                                host.fullName,
                                                style: new TextStyle(
                                                    color: CColors.TextBody,
                                                    fontFamily: "Roboto-Medium",
                                                    fontSize: 16
                                                )
                                            )
                                        ),
                                        host.title.isNotEmpty()
                                            ? new Container(
                                                child: new Text(
                                                    host.title,
                                                    maxLines: 1,
                                                    overflow: TextOverflow.ellipsis,
                                                    style: CTextStyle.PRegularBody3
                                                )
                                            )
                                            : new Container()
                                    }
                                )
                            }
                        ),
                        new Container(
                            margin: EdgeInsets.only(top: 8),
                            child: new Text(
                                host.description,
                                style: new TextStyle(
                                    color: CColors.TextBody3,
                                    fontFamily: "Roboto-Regular",
                                    fontSize: 16
                                )
                            )
                        )
                    }
                )
            );
        }
    }
}