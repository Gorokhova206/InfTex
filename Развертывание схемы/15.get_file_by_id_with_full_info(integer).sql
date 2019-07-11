-- Function: public.get_file_by_id_with_full_info(integer)

-- DROP FUNCTION public.get_file_by_id_with_full_info(integer);

CREATE OR REPLACE FUNCTION public.get_file_by_id_with_full_info(IN integer)
  RETURNS TABLE(id integer, name character, description character, type character, folder character, content text) AS
$BODY$
    SELECT fl.id, fl.name, fl.description, e.type,  fol.name, fl.content FROM file fl inner join extension e on (e.id = fl.extension_id) inner join folder fol on (fol.id = fl.folder_id) WHERE fl.id =$1;
$BODY$
  LANGUAGE sql VOLATILE
  COST 100
  ROWS 1000;
ALTER FUNCTION public.get_file_by_id_with_full_info(integer)
  OWNER TO postgres;
