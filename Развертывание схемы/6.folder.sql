-- Table: public.folder

-- DROP TABLE public.folder;

CREATE TABLE public.folder
(
  id integer NOT NULL DEFAULT nextval('file_id'::regclass), -- Код папки
  name character varying(100),
  parent_id integer,
  CONSTRAINT "FolerPK" PRIMARY KEY (id),
  CONSTRAINT "FolderFK" FOREIGN KEY (parent_id)
      REFERENCES public.folder (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE CASCADE
)
WITH (
  OIDS=FALSE
);
ALTER TABLE public.folder
  OWNER TO postgres;
COMMENT ON COLUMN public.folder.id IS 'Код папки';


-- Index: public.folder_index

-- DROP INDEX public.folder_index;

CREATE INDEX folder_index
  ON public.folder
  USING btree
  (id);

